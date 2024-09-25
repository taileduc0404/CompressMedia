using CompressMedia.Repositories.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace CompressMedia.Repositories
{
    public class MailKitEmailProvider : IEmailProvider
    {
        private readonly IConfiguration _configuration;

        public MailKitEmailProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string fullName, string to, string qrCodeUrl)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Compress Media Service", _configuration["EmailSettings:Sender"]));
            email.To.Add(new MailboxAddress(fullName, to));
            email.Subject = "QR Code";

            // Nội dung email với HTML đầy đủ
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>QR Code Email</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            background: white;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333;
                        }}
                        p {{
                            color: #555;
                        }}
                        a {{
                            color: blue;
                            text-decoration: none;
                            cursor: pointer; 
                        }}
                        a:hover {{
                            text-decoration: underline;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h1>QR Code Access</h1>
                        <p>Hello {fullName},</p>
                        <p>Link dẫn đến QR Code bên dưới, vui lòng copy rồi mở trong tab mới để quét :))) :</p>
                        <p><a href=""{qrCodeUrl}"" target=""_blank"" rel=""noopener noreferrer"">{qrCodeUrl}</a></p>   
                        <p>Thank you!</p>
                    </div>
                </body>
                </html>";

            email.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                try
                {
                    // Kết nối đến server SMTP
                    await smtpClient.ConnectAsync(
                        _configuration["EmailSettings:SmtpServer"],
                        int.Parse(_configuration["EmailSettings:Port"]!),
                        MailKit.Security.SecureSocketOptions.StartTls
                    );

                    // Xác thực tài khoản
                    await smtpClient.AuthenticateAsync(
                        _configuration["EmailSettings:Sender"],
                        _configuration["EmailSettings:Password"]
                    );

                    // Gửi email
                    await smtpClient.SendAsync(email);
                    await smtpClient.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Log exception
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
