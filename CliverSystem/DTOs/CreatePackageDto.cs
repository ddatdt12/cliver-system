using CliverSystem.Models;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class CreatePackageDto
    {
        public CreatePackageDto()
        {
        }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DeliveryTime { get; set; }
        public int? NumberOfPages { get; set; }
        public int? NumberOfRevisions { get; set; }
        public int Price { get; set; }
    }

}
