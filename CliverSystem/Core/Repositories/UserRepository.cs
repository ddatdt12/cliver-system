using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.User;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private ITransactionHistoryRepo _transactionHistoryRepo { get; set; }

        public UserRepository(DataContext context, ILogger logger, IMapper mapper, ITransactionHistoryRepo transactionHistoryRepo) : base(context, logger, mapper)
        {
            _transactionHistoryRepo = transactionHistoryRepo;
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
        public async Task<User> FindById(string id, string? logginUserId)
        {
            try
            {
                var user = await this.Find(u => u.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }
                if (logginUserId == null)
                {
                    user.IsSaved = false;
                }
                else
                {
                    user.IsSaved = await _context.SavedSellers.Where(ss => ss.UserId == id && ss.SavedList.UserId == logginUserId).AnyAsync();
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} FindUserByEmail function error", typeof(UserRepository));
                return null;
            }
        }

        public async Task VerifyAccount(string userId, VerifySellerDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ApiException("User doesn't exist", 404);
            }

            if (!dto.IdentityCardImage.StartsWith("https://"))
            {
                throw new ApiException("Invalid image url", 400);
            }
            user.IdentityCardImage = dto.IdentityCardImage;
            user.IsVerified = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CreateNewUser(User user)
        {
            user.Wallet = new Wallet();
            _context.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserDetails(string userId)
        {
            return await _context.Users.Where(w => w.Id == userId)
            .Include(u => u.Wallet).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserInfo(string userId)
        {
            return await _context.Users.Where(w => w.Id == userId).FirstOrDefaultAsync();
        }
        public async Task<bool> DepositMoneyIntoWallet(string userId, long amount)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var wallet = await _context.Wallets.Where(w => w.User!.Id == userId).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    throw new ApiException("Ví không hợp lệ", 400);
                }
                wallet!.Balance += amount;
                wallet!.AvailableForWithdrawal += amount;
                await _transactionHistoryRepo.CreateTransaction(new TransactionHistory
                {
                    Amount = amount,
                    Description = "Nạp tiền thành công vào ví",
                    Type = Common.Enum.TransactionType.Deposit,
                    WalletId = wallet.Id,
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        async Task IUserRepository.UpdateInfo(string userId, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            _mapper.Map(updateUserDto, user);

            await _context.SaveChangesAsync();
        }

        async Task<bool> IUserRepository.Withdraw(string userId, long amount)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var wallet = await _context.Wallets.Where(w => w.User!.Id == userId).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    throw new ApiException("Ví không hợp lệ", 400);
                }
                if (wallet.Balance < amount)
                {
                    throw new ApiException("Không đủ số dư", 400);
                }
                wallet!.Balance -= amount;
                wallet!.AvailableForWithdrawal -= amount;
                wallet!.Withdrawn += amount;
                await _transactionHistoryRepo.CreateTransaction(new TransactionHistory
                {
                    Amount = amount,
                    Description = "Rút tiền thành công về ví Vnpay",
                    Type = Common.Enum.TransactionType.Withdrawal,
                    WalletId = wallet.Id,
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<User>> Search(string search, string? userId)
        {
            return await _context.Users.Where(u => u.Id != userId && (u.Name.Contains(search) || u.Email.Contains(search))).ToListAsync();
        }
    }
}
