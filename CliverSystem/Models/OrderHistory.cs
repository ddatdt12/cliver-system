using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("OrderHistory")]
    public class OrderHistory
    {
        public OrderHistory()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        //public OrderStatus? BeforeStatus { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int? ResourceId { get; set; }
        public Resource? Resource { get; set; }
        public string? Message { get; set; }
    }

    public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> builder)
        {
        }
    }
}
