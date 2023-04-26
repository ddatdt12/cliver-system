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

    }
}
