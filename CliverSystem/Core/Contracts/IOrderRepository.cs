using CliverSystem.DTOs;
using CliverSystem.DTOs.Master;
using CliverSystem.Models;
using System.Linq.Expressions;

namespace CliverSystem.Core.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderById(int Id);
        Task InsertOrder(Order order, Common.Enum.PaymentMethod method);
        Task UpdateOrderPayment(int orderId);
        Task<List<Order>> GetOrders(string userId, Common.Enum.OrderStatus? status, Common.Enum.Mode mode = Common.Enum.Mode.Buyer);
        Task ReceiveOrder(int orderId, string userId);
        Task StartOrder(int orderId, string userId);
        Task PaymentOrderByWallet(int orderId, string userId);
        Task CancelOrder(int orderId, string userId, Common.Enum.Mode mode);
        Task DeliveryOrder(int orderId, string userId, CreateResouceDto? Resource = null);
        Task ReviseOrder(int orderId, string userId);
        Task<Order?> GetOrderByCustomPackageId(int packId);
    }
}
