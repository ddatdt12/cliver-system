using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    public class SavedList : AuditEntity
    {
        public SavedList()
        {
            SavedServices = new List<SavedService>();
            SavedSellers = new List<SavedSeller>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "varchar(36)")]
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        [NotMapped]
        public int ServiceCount { get; set; }
        [NotMapped]
        public int SellerCount { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; }
        [NotMapped]
        public string? Thumnail { get; set; }
        public ICollection<SavedService> SavedServices { get; set; }
        public ICollection<SavedSeller> SavedSellers { get; set; }
    }
}
