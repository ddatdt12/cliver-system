using CliverSystem.Models;

namespace CliverSystem.Core.Contracts
{
    public interface IAuthRepository
    {
        string GenerateToken(User user);
        string? ValidateToken(string token);
    }
}
