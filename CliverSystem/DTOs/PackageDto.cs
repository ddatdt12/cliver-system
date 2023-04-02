using CliverSystem.Models;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class PackageDto
    {
        public PackageDto()
        {
        }
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int DeliveryTime { get; set; }
        public int? NumberOfRevisions { get; set; }
        public int Price { get; set; }
        public PackageType Type { get; set; }
    }
}
