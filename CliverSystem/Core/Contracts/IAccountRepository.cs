using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface IAccountRepository : IGenericRepository<User>
    {
        Task UpdateBalance(string userId, long balance);
    }
}
