using CliverSystem.DTOs.Order;
using CliverSystem.DTOs.User;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class TransactionHistoryDto
    {
        public int Id { get; set; }
        public long Amount{ get; set; }
        public TransactionType Type{ get; set; }
        public int? OrderId { get; set; }
        public OrderDto? Order { get; set; }
        public int WalletId { get; set; }
        public WalletDto? Wallet{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
