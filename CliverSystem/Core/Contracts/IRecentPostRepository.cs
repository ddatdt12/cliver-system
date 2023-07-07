using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;

namespace CliverSystem.Core.Contracts
{
    public interface IRecentPostRepository : IGenericRepository<RecentPost>
    {
        Task CreateRecentPost(int postId, string userId);
        Task<IEnumerable<Post>> GetRecentPosts(string userId, RecentPostParameters param);
    }
}
