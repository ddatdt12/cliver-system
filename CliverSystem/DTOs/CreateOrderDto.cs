using System.Runtime.Serialization;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class CreateOrderDto
    {
        public CreateOrderDto()
        {
            Note = "";
        }
        public string Note { get; set; }
        public int PackageId { get; set; }
        //public List<OrderHistoryDto>? Histories { get; set; }
    }
}
