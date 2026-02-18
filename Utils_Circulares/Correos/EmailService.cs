using Entities_Circulares.EmailSettings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utils_Circulares.Correos
{
    public class EmailService
    {

        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(
            string subject,
            string body,
            bool isHtml,
            params string[] recipients)
        {
            if (recipients == null || recipients.Length == 0)
                throw new ArgumentException("Debe indicar al menos un destinatario");

            using var message = new MailMessage
            {
                From = new MailAddress(
                    _settings.SenderEmail,
                    _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var email in recipients)
            {
                message.To.Add(email);
            }

            using var smtp = new SmtpClient(
                _settings.SmtpServer,
                _settings.Port)
            {
                Credentials = new NetworkCredential(
                    _settings.Username,
                    _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            await smtp.SendMailAsync(message);
        }

    }
}
