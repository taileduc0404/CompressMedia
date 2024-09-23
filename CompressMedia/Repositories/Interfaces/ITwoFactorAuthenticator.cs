
namespace CompressMedia.Repositories.Interfaces
{
	public interface ITwoFactorAuthenticator
	{
		string GenerateOtp(string secretKey, int digits = 6, int timeStep = 30);
		string GenerateQrCode();
		bool VerifyOtpCode(int otpCode);
	}
}
