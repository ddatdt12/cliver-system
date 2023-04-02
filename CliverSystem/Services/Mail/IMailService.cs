using CliverSystem.DTOs;
using System.Threading.Tasks;

namespace CliverSystem.Services.Mail
{
    public interface IMailService
    {
        public Task SendRegisterMail(UserDto user, string token, bool isCreated = false);
        public Task SendForgotPasswordEmail(UserDto user, string token);
    }
}
