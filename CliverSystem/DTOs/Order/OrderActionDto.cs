using CliverSystem.DTOs.Master;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs.Order
{
    public class OrderActionDto
    {
        public OrderActionType Action { get; set; }
        public CreateResouceDto? Resource { get; set; }
    }
}
