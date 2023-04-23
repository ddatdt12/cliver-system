using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("User")]
    public class User : AuditEntity
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Posts = new HashSet<Post>();
            //Rooms = new HashSet<Room>();
            Type = UserType.User;
            IsActive = false;
            IsVerified = false;
        }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long NetIncome { get; set; }
        public long Withdrawn { get; set; }
        public long UsedForPurchases { get; set; }
        public long PendingClearance { get; set; }
        public long AvailableForWithdrawal { get; set; }
        public long ExpectedEarnings { get; set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Order>? Orders{ get; set; }
        //public ICollection<Room> Rooms { get; set; }
    }
}
