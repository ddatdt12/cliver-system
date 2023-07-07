using CliverSystem.DTOs;
using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<Review> CreateReview(int orderId, string userId, CreateReviewDto createDto, Common.Enum.Mode mode);
        Task<IEnumerable<Review>> GetReviewsOfUser(string userId);
        Task<IEnumerable<Review>> GetReviewsOfPost(int postId);
        Task<List<RatingStat>> GetReviewsStats(int postId);
        Task<List<RatingStat>> GetReviewsStatsOfUser(string userId);
        Task<List<Review>> GetReviewsSentiment(string userId);
    }
}
