using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;
using System.Linq.Expressions;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Core.Contracts
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<Post> FindById(int id);
        Task<PagedList<Post>> GetPosts(PostParameters postParameters,bool trackChanges = true);

        Task Update(int id, UpdatePostDto post);
        Task UpdateStatus(int id, PostStatus status);
    }
}
