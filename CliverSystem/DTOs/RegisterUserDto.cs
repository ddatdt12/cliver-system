
using System.ComponentModel.DataAnnotations;

namespace CliverSystem.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
