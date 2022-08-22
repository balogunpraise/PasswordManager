using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using System.Threading;
using MimeKit;

namespace PasswordManager.Api.Handlers
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmail(EmailModel emailModel)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSettings.Email)
            };
            email.To.Add(MailboxAddress.Parse(emailModel.ToEmail));
            email.Subject = emailModel.Subject;
            var builder = new BodyBuilder();
            if (emailModel.Files != null)
            {
                byte[] fileBytes;
                foreach (var file in emailModel.Files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = emailModel.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
