using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using LeaveManagement.Utils;

namespace LeaveManagement.Infrustructure.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (!IsValidEmail(email)) return;

            using var smtpClient = new SmtpClient();
            try
            {
                smtpClient.EnableSsl = EmailSettings.UseSsl;
                smtpClient.Host = EmailSettings.ServerName;
                smtpClient.Port = EmailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password);

                MailMessage mailMessage = new(EmailSettings.MailFromAddress, email)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"I am from EmailService SendEmailAsync, message : {ex.Message}");
            }
        }
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new MailAddress(email);
                return true; // Email is valid
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Not a valid email address . {email}, message : {ex.Message}");
                return false; // Email is invalid
            }
        }
    }
}
