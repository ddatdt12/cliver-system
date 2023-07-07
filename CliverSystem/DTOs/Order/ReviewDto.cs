using CliverSystem.DTOs.Order;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class ReviewDto
    {
        public ReviewDto()
        {
            UserId = null!;
            Comment = string.Empty;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Comment { get; set; }
        public OrderDto? Order { get; set; }
        public string UserId { get; set; }
        public UserDto? User { get; set; }
        public ReviewType Type { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
