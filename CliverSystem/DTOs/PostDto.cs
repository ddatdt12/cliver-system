using CliverSystem.Models;
using System.ComponentModel;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class PostDto : AuditEntity
    {
        public PostDto()
        {
            Subcategory = null!;
            Status = PostStatus.Draft;
            HasOfferPackages = false;
            Packages = new List<PackageDto>();
        }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = null!;
        public PostStatus Status { get; set; }
        public UserDto User { get; set; } = null!;
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; } = null!;
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
        public string? Video { get; set; }
        public string? Document { get; set; }
        public bool HasOfferPackages { get; set; }
        public ICollection<PackageDto> Packages { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

}
