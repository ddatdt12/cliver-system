using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.DTOs.User
{
    public class SavedListDto
    {
        public SavedListDto()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int ServiceCount { get; set; }
        public int SellerCount{ get; set; }
        public string? Thumnail { get; set; }
        public bool IsSaved{ get; set; }
    }
}
