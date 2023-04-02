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
        private IMapper _mapper;
        public PostRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<Post>> GetPosts(PostParameters postParameters, bool trackChanges)
        {
            var posts = await Find()
            .OrderBy(e => e.CreatedAt)
            .Skip((postParameters.PageNumber - 1) * postParameters.PageSize)
            .Take(postParameters.PageSize)
            .ToListAsync();

            var count = await Find(trackChanges: false).CountAsync();
            return new PagedList<Post>(posts, count, postParameters.PageNumber, postParameters.PageSize);
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
                var existP = await _context.Packages.Where(p => p.IsAvailable && p.Type == package.Type).FirstOrDefaultAsync();
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
        private List<string> RequiredFields = new List<string>() { "", "", "" };
        string CheckPostQualified(Post myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (RequiredFields.Any(rF => rF.Equals(pi.Name)))
                {
                    var value = pi.GetValue(myObject);
                    if (value == null)
                    {
                        return pi.Name +  " is required!";
                    }
                }
            }
            return null;
        }

        public async Task Update(int id, UpdatePostDto postData)
        {

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var post = await FindById(id);
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

                if (postData.IsPublish || post.Status != PostStatus.Draft)
                {
                    var messageError = CheckPostQualified(post);
                    if (messageError != null)
                    {
                        throw new ApiException(messageError, 400);
                    }

                    if (postData.IsPublish)
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

        public async Task<Post> FindById(int id)
        {
            return await _context.Posts.Where(p => p.Id == id).Include(p => p.Packages).Include(p => p.User).FirstOrDefaultAsync();
        }

        public async Task UpdateStatus(int id, PostStatus status)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post is null)
            {
                throw new ApiException("Post not found", 404);
            }

            if (post.Status == PostStatus.Draft)
            {
                throw new ApiException("Post is draft. Cannot update other status", 400);
            }
            post.Status = status;
            await _context.SaveChangesAsync();
        }

    }
}
