using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Crosscutting.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(string name, string toAddress, string subject, string body);
    }
    public class MailService : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailService(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            _smtpSettings = new SmtpSettings()
            {
                Host = configuration["SmtpSettings:Host"],
                Port = int.Parse(configuration["SmtpSettings:Port"]),
                SenderName = configuration["SmtpSettings:SenderName"],
                ServerEmail = configuration["SmtpSettings:ServerEmail"],
                Username = configuration["SmtpSettings:Username"],
                Password = configuration["SmtpSettings:Password"]
            };
        }

        public async Task SendEmailAsync(string name, string toAddress, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.ServerEmail));
            emailMessage.To.Add(new MailboxAddress(name, toAddress));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<h1>{subject}</h1><p>{body}</p>";
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            // Connect to the SMTP server (smtp4dev)
            await client.ConnectAsync("smtp4dev", _smtpSettings.Port, MailKit.Security.SecureSocketOptions.None);

            // Note: smtp4dev does not require authentication by default
            //await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
