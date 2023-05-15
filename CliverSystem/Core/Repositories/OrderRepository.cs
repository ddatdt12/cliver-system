using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Core.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private IMapper _mapper;
        public OrderRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger)
        {
            _mapper = mapper;
        }

        public async Task<List<Order>> GetOrders(string userId, Mode mode = Mode.Buyer)
        {
            var query = this.Find();
            if (mode == Mode.Buyer)
            {
                query = query.Where(o => o.BuyerId == userId);
            }
            else
            {
                query = query.Include(o => o.Package!.Post).Where(o => o.Package!.Post!.UserId == userId);
            }
            return await query.ToListAsync();
        }

        public async Task<Order> GetOrderById(int Id)
        {
            return await this.Find(o => o.Id == Id).Include(o => o.Buyer).Include(o => o.Package).FirstOrDefaultAsync();
        }
        private async Task CreateOrderHistory(OrderHistory his)
        {
            await _context.OrderHistories.AddAsync(his);
        }
        private async Task UpdateUserMoney(string userId, long money)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user!.Balance <= 0 )
            {
                throw new ApiException(message: "User don't have enough available balance for payment", statusCode: 400);
            }

            user.Balance -= money;
        }
        public async Task InsertOrder(Order order)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var package = await _context.Packages.FindAsync(order.PackageId);

                if (package == null || !package.IsAvailable)
                {
                    throw new ApiException(statusCode: 404, message: "Package you ordering does not exist");
                }

                order.Status = OrderStatus.Created;
                order.DueBy = DateTime.Now.Date.AddDays(package.DeliveryDays);
                order.RevisionTimes = package.NumberOfRevisions ?? 0;
                order.Price = package.Price;
                order.LockedMoney = package.Price;
                await UpdateUserMoney(order.BuyerId, order.Price);

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await CreateOrderHistory(new OrderHistory { BeforeStatus = null, CurrentStatus = OrderStatus.Created, CreatedAt = DateTime.Now, OrderId = order.Id });
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
