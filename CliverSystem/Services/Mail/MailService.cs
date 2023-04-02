using CliverSystem.DTOs;
using CliverSystem.Models.Settings;
using CliverSystem.Services.Mail;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CliverSystem.Services
{
    public partial class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendForgotPasswordEmail(UserDto user, string token)
        {
            try
            {
                MimeMessage mail = new MimeMessage();
                mail.To.Add(MailboxAddress.Parse(user.Email));
                mail.Subject = "T";
                var builder = new BodyBuilder();

                builder.HtmlBody = $@"Tesst";
                mail.Body = builder.ToMessageBody();
                await this.SendMail(mail);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public async Task SendRegisterMail(UserDto user, string token, bool isCreated = false)
        {
            try
            {
                MimeMessage mail = new MimeMessage();
                mail.To.Add(MailboxAddress.Parse(user.Email));
                mail.Subject = "[Cliver]Verification Email";
                var builder = new BodyBuilder();

                builder.HtmlBody = $"Verification Code <h1>{token}</h1>";
                mail.Body = builder.ToMessageBody();
                await this.SendMail(mail);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
