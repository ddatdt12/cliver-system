using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs.Order
{
    public class OrderDto : BaseDto
    {
        public OrderDto()
        {
            Note = "";
            BuyerId = null!;
            SellerId = null!;
            Histories = new List<OrderHistoryDto>();
        }
        public int Id { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public DateTime DueBy { get; set; }
        public string BuyerId { get; set; }
        public UserDto? Buyer { get; set; }
        public string SellerId { get; set; }
        public UserDto? Seller{ get; set; }
        public int RevisionTimes { get; set; }
        public int LeftRevisionTimes { get; set; }
        public int PackageId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PackageDto? Package { get; set; }
        public OrderStatus? Status { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public List<OrderHistoryDto> Histories { get; set; }
    }
}
