using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindUserByEmailAndPassword(string email, string password);
        Task<User> FindByEmail(string email);
        Task VerifyAccount(string email);
    }
}
