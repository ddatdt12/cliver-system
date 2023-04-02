using System.ComponentModel.DataAnnotations;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class UserDto
    {
        public UserDto()
        {
            Posts = new List<PostDto>();
        }

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
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
        public ICollection<PostDto> Posts { get; set; }
    }
}
