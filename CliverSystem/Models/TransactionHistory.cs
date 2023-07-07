using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table(name: "TransactionHistory")]
    public class TransactionHistory
    {
        public TransactionHistory()
        {
            CreatedAt = DateTime.UtcNow;
            Description = string.Empty;
        }
        public int Id { get; set; }
        public long Amount{ get; set; }
        public string Description{ get; set; }
        public TransactionType Type{ get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public int WalletId { get; set; }
        public Wallet? Wallet{ get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
