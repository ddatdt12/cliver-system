using CliverSystem.DTOs.Master;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs.Order
{
    public class OrderHistoryDto
    {
        public OrderHistoryDto()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        //public OrderStatus? BeforeStatus { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ResourceId{ get; set; }
        public ResourceDto? Resource{ get; set; }
        public int OrderId { get; set; }
    }
}
