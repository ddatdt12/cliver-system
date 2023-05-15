using CliverSystem.DTOs;
using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
  public interface IOrderRepository : IGenericRepository<Order>
  {
    Task<List<Order>> GetOrders(string userId, Common.Enum.Mode mode);
    Task<Order> GetOrderById(int Id);
    Task InsertOrder(Order order);
  }
}
