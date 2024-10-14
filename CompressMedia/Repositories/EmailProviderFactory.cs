using CompressMedia.Repositories.Interfaces;

namespace CompressMedia.Repositories
{
    public class EmailProviderFactory
    {
        private readonly IConfiguration _configuration;

        public EmailProviderFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEmailProvider CreateEmailProvider()
        {
            var provider = _configuration["EmailSettings:Provider"];

            return provider switch
            {
                "MailKit" => new MailKitEmailProvider(_configuration),
                "Smtp" => new SmtpEmailProvider(_configuration),
                _ => throw new NotSupportedException("Email Provider Not Supported.")
            };
        }
    }
}
