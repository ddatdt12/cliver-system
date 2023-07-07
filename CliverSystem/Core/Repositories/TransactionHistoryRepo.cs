using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CliverSystem.Core.Repositories
{
    public class TransactionHistoryRepo : GenericRepository<TransactionHistory>, ITransactionHistoryRepo
    {
        public TransactionHistoryRepo(DataContext context, ILogger logger, IMapper mapper)
        : base(context, logger, mapper)
        {

        }

        public async Task<IEnumerable<TransactionHistory>> GetTransactions(int walletId)
        {
            var tranHises = await _context.TransactionHistories
            .Where(tH => tH.WalletId == walletId).OrderByDescending(tH => tH.CreatedAt).ToListAsync();
            return tranHises;
        }

        public async Task CreateTransaction(TransactionHistory dtoTrans)
        {
            await _context.TransactionHistories.AddAsync(dtoTrans);
            await _context.SaveChangesAsync();
        }
    }
}
