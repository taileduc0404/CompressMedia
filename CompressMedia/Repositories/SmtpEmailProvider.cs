using CompressMedia.Repositories.Interfaces;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace CompressMedia.Repositories
{
    public class SmtpEmailProvider : IEmailProvider
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string fullName, string to, string otp)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]!))
            {
                Credentials = new NetworkCredential(_configuration["EmailSettings:Sender"], _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:Sender"]!, "Compress Media Service"),
                Subject = "OTP Code",
                Body = $"Your OTP: {otp}",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(to, fullName));
            await smtpClient.SendMailAsync(message);
        }
    }
}
