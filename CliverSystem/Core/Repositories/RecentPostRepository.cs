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
    public class RecentPostRepository : GenericRepository<RecentPost>, IRecentPostRepository
    {
        public RecentPostRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task CreateRecentPost(int postId, string userId)
        {
            var recentPost = await _context.RecentPosts.Where(rp => rp.PostId == postId && rp.UserId == userId).FirstOrDefaultAsync();
            if (recentPost != null)
            {
                recentPost.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                _context.RecentPosts.Add(new RecentPost
                {
                    PostId = postId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Post>> GetRecentPosts(string userId, RecentPostParameters param)
        {
            var recentPostQuery = _context.RecentPosts.Where(rp => rp.UserId == userId);
            var recentPosts = await recentPostQuery
            .Include(p => p.Post)
             .ThenInclude(p => p!.Packages)
             .Include(p => p.Post)
             .ThenInclude(p => p!.User)
             .OrderByDescending(rp => rp.CreatedAt)
             .Select(p => p.Post!).AsNoTracking()
             .ToListAsync();
            int totalCount = recentPosts.Count();

            if (userId != null)
            {
                var postIds = recentPosts.Select(p => p.Id);

                var savedServiceUserMap = (await _context.SavedServices
                .Where(sS => sS.SavedList!.UserId == userId && postIds.Contains(sS.PostId))
                //.ToDictionaryAsync();
                .ToListAsync()).DistinctBy(sS => sS.PostId).ToDictionary(sS => sS.PostId, sS => sS);

                foreach (var p in recentPosts)
                {
                    p.IsSaved = savedServiceUserMap.ContainsKey(p.Id);
                }
            }
            return new PagedList<Post>(recentPosts, totalCount, param.Offset, param.Limit);
        }
    }
}
