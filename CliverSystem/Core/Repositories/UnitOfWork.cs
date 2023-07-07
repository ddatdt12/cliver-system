using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.Models;

namespace CliverSystem.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        public IUserRepository Users { get; private set; }
        public IAccountRepository Accounts { get; private set; }
        public IAuthRepository Auth { get; private set; }
        public IPostRepository Posts { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public IOrderRepository Orders { get; private set; }
        public IRoomRepository Rooms { get; private set; }
        public IMessageRepository Messages { get; private set; }
        public IRecentPostRepository RecentPosts { get; private set; }
        public ISavedListRepository SavedLists { get; private set; }
        public IServiceRequestRepo ServiceRequests{ get; private set; }
        public ITransactionHistoryRepo TransactionHistories{ get; private set; }
        public IReviewRepository Reviews{ get; private set; }

        public UnitOfWork(DataContext context, ILoggerFactory loggerFactory
                , IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            RecentPosts = new RecentPostRepository(context, _logger,mapper);
            TransactionHistories = new TransactionHistoryRepo(context, _logger, mapper);
            Users = new UserRepository(context, _logger, mapper, TransactionHistories);
            Posts = new PostRepository(context, _logger, mapper, RecentPosts);
            Accounts = new AccountRepository(context, _logger,mapper);
            Auth = new AuthRepository(configuration, context, _logger);
            Categories = new CategoryRepository(context, _logger,mapper);
            Orders = new OrderRepository(context, _logger, mapper, TransactionHistories);
            Rooms = new RoomRepository(context, _logger,mapper);
            Messages = new MessageRepository(context, _logger, mapper);
            SavedLists = new SavedListRepository(context, _logger,mapper);
            ServiceRequests = new ServiceRequestRepo(context, _logger, mapper);
            Reviews = new ReviewRepository(context, _logger, mapper);
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
