using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface ITransactionHistoryRepo : IGenericRepository<TransactionHistory>
    {
        Task CreateTransaction(TransactionHistory dtoTrans);
        Task<IEnumerable<TransactionHistory>> GetTransactions(int walletId);
    }
}
