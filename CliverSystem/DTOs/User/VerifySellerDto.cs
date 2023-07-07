using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.DTOs.User
{
    public class VerifySellerDto
    {
        public VerifySellerDto()
        {
            IdentityCardImage = string.Empty;
        }
        [Required]
        public string IdentityCardImage { get; set; }
    }
}
