using CliverSystem.Models;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class UpsertPackageDto
    {
        public UpsertPackageDto()
        {
        }
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? PostId { get; set; }
        public int DeliveryDays { get; set; }
        public int? NumberOfRevisions { get; set; }
        public int Price { get; set; }
        public PackageType Type { get; set; }
    }

    class UpsertPackageDtoEqualityComparer : IEqualityComparer<UpsertPackageDto>
    {
        public bool Equals(UpsertPackageDto? x, UpsertPackageDto? y)
        {
            if ((x?.Id.HasValue != y?.Id.HasValue && x!.Id != y!.Id) || x?.Type != y?.Type)
            {
                return false;
            }
            return true;
        }

        public int GetHashCode(UpsertPackageDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
