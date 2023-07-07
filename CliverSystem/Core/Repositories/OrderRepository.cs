using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Master;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using static CliverSystem.Common.Enum;
namespace CliverSystem.Core.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private ITransactionHistoryRepo _transactionHistoryRepo { get; set; }
        public OrderRepository(DataContext context, ILogger logger, IMapper mapper, ITransactionHistoryRepo transactionHistoryRepo) : base(context, logger, mapper)
        {
            _transactionHistoryRepo = transactionHistoryRepo;
        }

        public async Task<List<Order>> GetOrders(string userId, OrderStatus? status, Mode mode = Mode.Buyer)
        {
            var query = this.Find(trackChanges: false).IgnoreQueryFilters();
            if (mode == Mode.Buyer)
            {
                query = query.Where(o => o.BuyerId == userId).Include(o => o.Seller);
            }
            else
            {
                query = query.Where(o => o.SellerId == userId && o.Status != OrderStatus.PendingPayment).Include(o => o.Buyer);
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status);
            }
            return await query.OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Package)
            .ThenInclude(p => p!.Post)
            .ToListAsync();
        }

        public async Task<Order?> GetOrderByCustomPackageId(int packId)
        {
            var existCustomPackage = await _context.Packages
            .IgnoreQueryFilters().Where(p => p.Id == packId && p.Type == PackageType.Custom && p.Status == PackageStatus.Ordered)
            .AnyAsync();
            if (!existCustomPackage)
            {
                throw new ApiException("Package not found", 400);
            }

            var order = await this.Find(o => o.PackageId == packId)
            .IgnoreQueryFilters()
            .Include(o => o.Buyer)
            .Include(o => o.Seller)
            .Include(o => o.Package).ThenInclude(p => p.Post)
            .Include(o => o.Histories!).ThenInclude(h => h.Resource)
            .Include(o => o.Reviews)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            return order;
        }
        public async Task<Order> GetOrderById(int Id)
        {
            var order = await this.Find(o => o.Id == Id)
            .IgnoreQueryFilters()
            .Include(o => o.Buyer)
            .Include(o => o.Seller)
            .Include(o => o.Package).ThenInclude(p => p.Post)
            .Include(o => o.Histories!).ThenInclude(h => h.Resource)
            .Include(o => o.Reviews)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            return order;
        }
        private async Task CreateOrderHistory(OrderHistory his)
        {
            await _context.OrderHistories.AddAsync(his);
        }
        private async Task UpdateUserMoney(string userId, long money, int orderId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).Include(u => u.Wallet).FirstOrDefaultAsync();

            if (user?.Wallet == null || user!.Wallet!.AvailableForWithdrawal <= 0)
            {
                throw new ApiException(message: "User don't have enough available balance for payment", statusCode: 400);
            }

            user!.Wallet!.Balance -= money;
            user!.Wallet!.AvailableForWithdrawal -= money;

            await _transactionHistoryRepo.CreateTransaction(new TransactionHistory
            {
                Amount = money,
                Description = "Successful payment",
                Type = Common.Enum.TransactionType.Payment,
                WalletId = user.WalletId,
                OrderId = orderId,
            });
        }
        public async Task InsertOrder(Order order, PaymentMethod method)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var package = await _context.Packages.Include(p => p.Post)
                   .IgnoreQueryFilters()
                   .Where(o => o.Id == order.PackageId).FirstOrDefaultAsync();

                if (package == null || !package.IsAvailable)
                {
                    throw new ApiException(statusCode: 404, message: "Package you ordering does not exist");
                }
                if (order.BuyerId == package.Post!.UserId)
                {
                    throw new ApiException(statusCode: 400, message: "You cannot buy your service!");
                }

                if (package!.Type == PackageType.Custom)
                {
                    package.Status = PackageStatus.Ordered;
                }

                order.DueBy = DateTime.UtcNow.Date.AddDays(package.DeliveryDays);
                order.RevisionTimes = package.NumberOfRevisions ?? 0;
                order.LeftRevisionTimes = package.NumberOfRevisions ?? 0;
                order.Price = package.Price;
                order.LockedMoney = package.Price;
                order.PaymentMethod = method;
                order.SellerId = package.Post!.UserId;

                if (method == PaymentMethod.MyWallet)
                {
                    order.Status = OrderStatus.Created;
                }
                else
                {
                    order.Status = OrderStatus.PendingPayment;
                }



                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                if (method == PaymentMethod.MyWallet)
                {
                    await UpdateUserMoney(order.BuyerId, order.Price, order.Id);
                }
                await CreateOrderHistory(new OrderHistory { Status = order.Status ?? OrderStatus.Created, CreatedAt = DateTime.UtcNow, OrderId = order.Id });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateOrderPayment(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Where(o => o.Id == orderId).Include(o => o.Buyer).FirstOrDefaultAsync();

                if (order == null || order.Status != OrderStatus.PendingPayment)
                {
                    throw new ApiException(statusCode: 404, message: "Order does not exist or status invalid");
                }
                await _transactionHistoryRepo.CreateTransaction(new TransactionHistory
                {
                    Amount = order.Price,
                    Description = "Successful payment",
                    Type = Common.Enum.TransactionType.Payment,
                    WalletId = order.Buyer!.WalletId,
                    OrderId = orderId,
                });

                order.Status = OrderStatus.Created;
                await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Created, CreatedAt = DateTime.UtcNow, OrderId = order.Id });
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CancelOrder(int orderId, string userId, Mode mode)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
                if (order == null)
                {
                    throw new ApiException("Order is invalid!", 400);
                }
                if (mode == Mode.Buyer)
                {
                    if (order.BuyerId != userId)
                    {
                        throw new ApiException("You are not authorized to do this action!", 400);
                    }

                    if (order.Status != OrderStatus.PendingPayment && order.Status != OrderStatus.Created)
                    {
                        throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
                    }
                }
                else
                {
                    if (order.SellerId != userId)
                    {
                        throw new ApiException("You are not authorized to do this action!", 400);
                    }
                    List<OrderStatus> validStatuses = new List<OrderStatus>() {
                    OrderStatus.Created,
                };
                    if (!validStatuses.Contains(order.Status ?? OrderStatus.PendingPayment))
                    {
                        throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
                    }
                }

                var wallet = await _context.Wallets.Where(w => w.User!.Id == order.BuyerId).FirstOrDefaultAsync();

                if (order.Status != OrderStatus.PendingPayment)
                {
                    await _transactionHistoryRepo.CreateTransaction(new TransactionHistory
                    {
                        Amount = order.Price,
                        Type = TransactionType.Refund,
                        WalletId = wallet!.Id,
                        OrderId = order.Id,
                        Description = "Order Canceled",
                    });
                    wallet.Balance += order.Price;
                    wallet.AvailableForWithdrawal += order.Price;
                }
                order.Status = OrderStatus.Cancelled;

                await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Cancelled, CreatedAt = DateTime.UtcNow, OrderId = order.Id });

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task StartOrder(int orderId, string userId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new ApiException("Order is invalid!", 400);
            }

            if (order.SellerId != userId)
            {
                throw new ApiException("You are not authorized to do this action!", 400);
            }

            if (order.Status != OrderStatus.Created)
            {
                throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
            }

            order.Status = OrderStatus.Doing;
            await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Doing, CreatedAt = DateTime.UtcNow, OrderId = order.Id });
            await _context.SaveChangesAsync();
        }
        public async Task DeliveryOrder(int orderId, string userId, CreateResouceDto? resource = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
                if (order == null)
                {
                    throw new ApiException("Order is invalid!", 400);
                }

                if (order.SellerId != userId)
                {
                    throw new ApiException("You are not authorized to do this action!", 400);
                }

                if (order.Status != OrderStatus.Doing)
                {
                    throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
                }

                var his = new OrderHistory { Status = OrderStatus.Delivered, CreatedAt = DateTime.UtcNow, OrderId = order.Id };

                if (resource != null)
                {
                    var newResource = new Resource { Name = resource.Name, Size = resource.Size, Url = resource.Url };
                    await _context.Resources.AddAsync(newResource);
                    await _context.SaveChangesAsync();
                    his.ResourceId = newResource.Id;
                }

                order.Status = OrderStatus.Delivered;
                await CreateOrderHistory(his);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task ReceiveOrder(int orderId, string userId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new ApiException("Order is invalid!", 400);
            }

            if (order.BuyerId != userId)
            {
                throw new ApiException("You are not authorized to do this action!", 400);
            }

            if (order.Status != OrderStatus.Delivered)
            {
                throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
            }
            var wallet = await _context.Wallets.Where(w => w.User.Id == order.SellerId).FirstOrDefaultAsync();

            wallet!.AvailableForWithdrawal += order.Price;
            wallet.Balance += order.Price;
            order.Status = OrderStatus.Completed;
            await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Completed, CreatedAt = DateTime.UtcNow, OrderId = order.Id });
            await _context.SaveChangesAsync();
        }
        public async Task ReviseOrder(int orderId, string userId)
        {
            var order = await _context.Orders.Where(o => o.Id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new ApiException("Order is invalid!", 400);
            }

            if (order.BuyerId != userId)
            {
                throw new ApiException("You are not authorized to do this action!", 400);
            }

            if (order.Status != OrderStatus.Delivered)
            {
                throw new ApiException("Order, being currently in this status, cannot be cancelled!", 400);
            }
            if (order.LeftRevisionTimes <= 0)
            {
                throw new ApiException("You have run out of revisions", 400);
            }
            order.Status = OrderStatus.Doing;
            order.LeftRevisionTimes--;
            await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Doing, CreatedAt = DateTime.UtcNow, OrderId = order.Id });
            await _context.SaveChangesAsync();
        }

        public async Task PaymentOrderByWallet(int orderId, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Where(o => o.Id == orderId && o.BuyerId == userId).FirstOrDefaultAsync();
                if (order == null)
                {
                    throw new ApiException("Order not found", 404);
                }
                if (order.Status != OrderStatus.PendingPayment)
                {
                    throw new ApiException("Order was already paid", 400);
                }
                order.Status = OrderStatus.Created;
                order.PaymentMethod = PaymentMethod.MyWallet;
                await UpdateUserMoney(order.BuyerId, order.Price, order.Id);
                await CreateOrderHistory(new OrderHistory { Status = OrderStatus.Created, CreatedAt = DateTime.UtcNow, OrderId = order.Id });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
