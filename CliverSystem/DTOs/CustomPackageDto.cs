using CliverSystem.Models;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class CustomPackageDto
    {
        public CustomPackageDto()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PostId { get; set; }
        public SimplePostDto? Post { get; set; }
        public bool IsAvailable { get; set; }
        public int DeliveryDays { get; set; }
        public int? NumberOfRevisions { get; set; }
        public int Price { get; set; }
        public PackageStatus Status { get; set; }
        public PackageType Type { get; set; }
        public int? ExpirationDays { get; set; }

    }
}
