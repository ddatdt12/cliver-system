using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<User> FindUserByEmailAndPassword(string email, string password)
        {
            try
            {
                return await this.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return null;
            }
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return new List<User>();
            }
        }

        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existingUser = await dbSet.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingUser == null)
                    return await Add(entity);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(UserRepository));
                return false;
            }
        }

        public async Task<User> FindByEmail(string email)
        {
            try
            {
                return await this.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} FindUserByEmail function error", typeof(UserRepository));
                return null;
            }
        }

        public async Task VerifyAccount(string email)
        {
            var user = await FindByEmail(email);
            user.IsVerified = true;
            await _context.SaveChangesAsync();
        }
    }
}
