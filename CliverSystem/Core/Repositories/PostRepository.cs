using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Core.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private IRecentPostRepository _recentPostRepository;
        public PostRepository(DataContext context, ILogger logger, IMapper mapper, IRecentPostRepository recentPostRepository)
        : base(context, logger, mapper)
        {
            _recentPostRepository = recentPostRepository;
        }

        public async Task<PagedList<Post>> GetPosts(PostParameters postParameters, string? userId = null, bool trackChanges = false)
        {
            var postsQuery = Find(p => p.Status == PostStatus.Active, trackChanges: false)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(postParameters.Offset)
            .Take(postParameters.Limit)
            .Include(e => e.User)
            .Include(p => p.Packages.OrderBy(p => p.Price)).AsQueryable();

            if (!string.IsNullOrEmpty(postParameters.Search))
            {
                string textSearch = postParameters.Search;
                if (textSearch.Length <= 2)
                {
                    throw new ApiException("Text search length must be greater then 2", 400);
                }

                postsQuery = postsQuery.Where(p => p.Title.Contains(textSearch)
                || p.Description.Contains(textSearch)
                || p.Packages.Any(p => p.Name.Contains(textSearch)) || p.Subcategory!.Name.Contains(textSearch));
            }

            if (postParameters.SubCategoryId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.SubcategoryId == postParameters.SubCategoryId);
            }
            else if (postParameters.CategoryId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Subcategory!.CategoryId == postParameters.CategoryId);
            }

            if (postParameters.Filter.HasValue)
            {
                switch (postParameters.Filter)
                {
                    case PostFilter.Relevance:

                        break;
                    case PostFilter.BestSelling:

                        break;
                    case PostFilter.NewArrivals:
                        postsQuery = postsQuery.OrderByDescending(p => p.CreatedAt);
                        break;
                }
            }

            if (postParameters.MaxPrice.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Packages.Max(p => p.Price) <= postParameters.MaxPrice);
            }

            if (postParameters.MinPrice.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Packages.Min(p => p.Price) >= postParameters.MaxPrice);
            }

            if (postParameters.DeliveryTime.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Packages.Any(p => p.DeliveryDays <= postParameters.DeliveryTime));
            }

            var posts = await postsQuery.ToListAsync();
            if (userId != null)
            {
                var postIds = posts.Select(p => p.Id);

                var savedServiceUserMap = (await _context.SavedServices
                .Where(sS => sS.SavedList!.UserId == userId && postIds.Contains(sS.PostId))
                //.ToDictionaryAsync();
                .ToListAsync()).DistinctBy(sS => sS.PostId).ToDictionary(sS => sS.PostId, sS => sS);

                foreach (var p in posts)
                {
                    p.IsSaved = savedServiceUserMap.ContainsKey(p.Id);
                }
            }



            var count = await Find(p => p.Status == PostStatus.Active, trackChanges: false).CountAsync();
            return new PagedList<Post>(posts, count, postParameters.Offset, postParameters.Limit);
        }
        private async Task UpsertPackage(UpsertPackageDto package)
        {
            if (package.Id.HasValue)
            {
                var pack = await _context.Packages.FindAsync(package.Id);
                if (pack != null)
                {
                    throw new ApiException("Package not found", 400);
                }

                _mapper.Map(package, pack);
            }
            else
            {
                var existP = await _context.Packages.Where(p => p.IsAvailable && p.Type == package.Type && p.PostId == package.PostId).FirstOrDefaultAsync();
                if (existP != null)
                {
                    _mapper.Map(package, existP);
                }
                else
                {
                    var pack = new Package();
                    _mapper.Map(package, pack);
                    _context.Packages.Add(pack);
                }

            }
        }
        private List<string> RequiredFields = new List<string>() { "Tags", "Images", "Video", "Document" };
        string CheckPostQualified(Post myObject)
        {
            if (myObject.Images.Count() < 1)
            {
                return "Provide at least 1 image about your service";
            }
            if (myObject.HasOfferPackages == false && myObject.Packages.Count() < 1)
            {
                return "Provide at least 1 package for service";
            }
            if (myObject.HasOfferPackages && myObject.Packages.Count() != 3)
            {
                return "Provide 3 package for service";
            }
            return null;
        }

        public async Task Update(int id, UpdatePostDto postData)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var post = await _context.Posts.Where(p => p.Id == id).IgnoreQueryFilters().FirstOrDefaultAsync();
                if (post == null)
                {
                    throw new ApiException("Post not found", 404);
                }

                if (postData.HasOfferPackages != null)
                {
                    if (postData.HasOfferPackages.Value)
                    {
                        if (postData.Packages is null)
                        {
                            throw new ApiException("Invalid packages data", 400);
                        }
                        postData.Packages = new HashSet<UpsertPackageDto>(postData.Packages.ToList(), new UpsertPackageDtoEqualityComparer()).ToList();
                        if (postData.Packages.Count() < 3)
                        {
                            throw new ApiException("Post must have 3 packages", 400);
                        }
                        foreach (var item in postData.Packages)
                        {
                            item.PostId = id;
                            await UpsertPackage(item);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await _context.Entry(post).Collection(p => p.Packages).Query()
                               .Where(p => p.IsAvailable)
                               .LoadAsync();

                        bool needSaveChanges = false;
                        var basicPackage = postData.Packages?.Where(p => p.Type == PackageType.Basic).FirstOrDefault();
                        if (basicPackage != null)
                        {
                            needSaveChanges = true;
                            basicPackage.PostId = post.Id;

                            await UpsertPackage(basicPackage);
                        }
                        if (post.Packages.Count() > 1)
                        {
                            var packs = post.Packages.Where(p => p.Type == PackageType.Premium || p.Type == PackageType.Standard);

                            foreach (var p in packs)
                            {
                                _context.Packages.Remove(p);
                            }
                            needSaveChanges = true;
                        }
                        if (needSaveChanges) await _context.SaveChangesAsync();
                    }
                    postData.Packages = null;
                }

                _mapper.Map(postData, post);

                if ((postData.IsPublish.HasValue && (bool)postData.IsPublish) || post.Status != PostStatus.Draft)
                {
                    await _context.Entry(post).Collection(s => s.Packages).LoadAsync();
                    var messageError = CheckPostQualified(post);
                    if (messageError != null)
                    {
                        throw new ApiException(messageError, 400);
                    }

                    if (postData.IsPublish.HasValue && (bool)postData.IsPublish && post.Status != PostStatus.Active)
                    {
                        post.Status = PostStatus.Active;
                    }
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Post> FindById(int id, string? userId = null)
        {
            var post = await _context.Posts.Where(p => p.Id == id)
            .IgnoreQueryFilters()
            .Include(p => p.Packages.Where(p => p.Type != PackageType.Custom))
            .Include(p => p.User)
            .FirstOrDefaultAsync();

            if (post != null && userId != null && userId != post.UserId)
            {
                //User Message Queue in here to handle async task
                await _recentPostRepository.CreateRecentPost(id, userId);

                post.IsSaved = await _context.SavedServices.Where(sS => post.Id == sS.PostId && sS.SavedList!.UserId == userId).AnyAsync();
            }
            return post;
        }

        public async Task UpdateStatus(int id, string userId, PostStatus status)
        {
            var post = await _context.Posts.IgnoreQueryFilters().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (post is null)
            {
                throw new ApiException("Post not found", 404);
            }

            if (post.UserId != userId)
            {
                throw new ApiException("You are not authorized!", 404);
            }

            if (post.Status == PostStatus.Draft)
            {
                throw new ApiException("Post is draft. Cannot update other status", 400);
            }
            if (post.Status != status)
            {
                post.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<Post>> GetPostsByUser(string userId, PostParameters postParameters)
        {
            var postsQuery = Find(trackChanges: false)
            .Where(p => p.UserId == userId)
                .IgnoreQueryFilters()
                .OrderByDescending(e => e.CreatedAt)
                .Include(e => e.User)
                .Include(p => p.Packages.OrderBy(p => p.Price)).AsQueryable();

            if (postParameters.Status.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Status == postParameters.Status);
            }
            var posts = await postsQuery.ToListAsync();

            return new PagedList<Post>(posts, posts.Count, 0, posts.Count);
        }

        async Task IPostRepository.DeletePost(int id, string userId)
        {
            var post = await _context.Posts.Where(p => p.Id == id).IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync();
            if (post == null)
            {
                throw new ApiException("Post not found!", 404);
            }
            if (post.Status != PostStatus.Draft)
            {
                throw new ApiException("Only draft status can be deleted!", 400);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
