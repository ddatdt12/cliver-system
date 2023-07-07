using CliverSystem.Core.Contracts;

namespace CliverSystem.Core.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IAuthRepository Auth { get; }
        IPostRepository Posts { get; }
        IAccountRepository Accounts { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IMessageRepository Messages { get; }
        IRoomRepository Rooms { get; }
        IRecentPostRepository RecentPosts { get; }
        ISavedListRepository SavedLists{ get; }
        IServiceRequestRepo ServiceRequests { get; }
        ITransactionHistoryRepo TransactionHistories{ get; }
        IReviewRepository Reviews{ get; }

        Task CompleteAsync();
    }
}
