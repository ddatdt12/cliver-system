using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("Order")]
    public class Order : AuditEntity
    {
        public Order()
        {
            Buyer = null;
            Note = "";
            BuyerId = "";
        }
        public int Id { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public DateTime DueBy { get; set; }
        public string BuyerId { get; set; }
        public User? Buyer { get; set; }
        public int RevisionTimes { get; set; }
        public int PackageId { get; set; }
        public Package? Package { get; set; }
        public OrderStatus? Status { get; set; }
        public List<OrderHistory>? Histories { get; set; }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(b => b.Package).WithMany(p => p.Orders).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.Buyer).WithMany(p => p.Orders).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
