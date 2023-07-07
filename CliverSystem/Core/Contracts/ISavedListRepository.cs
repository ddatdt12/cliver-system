using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface ISavedListRepository : IGenericRepository<SavedList>
    {
        Task<SavedList> CreateNewList(string userId, string name);
        Task<bool> DeleteList(int id);
        Task<ICollection<SavedList>> GetMyLists(string userId, int? postId, string? sellerId);
        Task<IEnumerable<User>> GetSavedSellers(int savedListId);
        Task<IEnumerable<Post>> GetSavedServices(int savedListId);
        Task<bool> SaveOrRemoveSeller(int savedListId, string sellerId, string userId);
        Task<bool> SaveOrRemoveService(int savedListId, int postId, string userId);
        Task<SavedList> UpdateSavedList(int id, string userId, string name);
        Task<IEnumerable<Post>> GetRecentlySavedServices(string userId);
    }
}
