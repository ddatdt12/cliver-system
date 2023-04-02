using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CliverSystem.Services
{
    public partial class MailService
    {
        public async Task SendMail(MimeMessage email)
        {
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
