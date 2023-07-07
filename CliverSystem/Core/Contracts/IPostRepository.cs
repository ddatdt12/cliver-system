using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;
using System.Linq.Expressions;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Core.Contracts
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<Post> FindById(int id, string? userId = null);
        Task<PagedList<Post>> GetPosts(PostParameters postParameters, string? userId = null, bool trackChanges = false);
        Task<PagedList<Post>> GetPostsByUser(string userId, PostParameters postParameters);
        Task Update(int id, UpdatePostDto post);
        Task DeletePost(int id, string userId);
        Task UpdateStatus(int id, string userId, PostStatus status);
    }
}
