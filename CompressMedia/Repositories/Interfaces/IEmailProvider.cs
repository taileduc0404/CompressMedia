namespace CompressMedia.Repositories.Interfaces
{
    public interface IEmailProvider
    {
        Task SendEmailAsync(string fullName, string to, string otp);
    }
}
