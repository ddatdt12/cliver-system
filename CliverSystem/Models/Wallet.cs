using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Wallet")]
    public class Wallet
    {
        public Wallet()
        {
            TransactionHistories=  new List<TransactionHistory>();
        }
        public int Id{ get; set; }
        public long Balance { get; set; }
        public long NetIncome { get; set; }
        public long Withdrawn { get; set; }
        public long UsedForPurchases { get; set; }
        public long PendingClearance { get; set; }
        public long AvailableForWithdrawal { get; set; }
        public long ExpectedEarnings { get; set; }
        public User? User { get; set; }
        public ICollection<TransactionHistory> TransactionHistories{ get; set; }
    }
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasOne(w => w.User).WithOne(u => u.Wallet).HasForeignKey<User>(u => u.WalletId);    
        }
    }
}
