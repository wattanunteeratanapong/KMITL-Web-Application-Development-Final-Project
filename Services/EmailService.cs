using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Togeta.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendNotificationEmail(string toEmail, string subject, string body)
        {
            // Retrieve Gmail settings from configuration
            string smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            string smtpUser = _configuration["EmailSettings:SmtpUser"];
            string smtpPass = _configuration["EmailSettings:SmtpPass"];
            string senderEmail = _configuration["EmailSettings:SenderEmail"];

            using (var smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true; // Gmail requires SSL/TLS

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                smtpClient.Send(mailMessage);
            }
        }
    }
}
