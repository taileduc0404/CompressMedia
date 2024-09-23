using CompressMedia.Repositories.Interfaces;


namespace CompressMedia.Repositories
{
	public class TwoFactorAuthenticator : ITwoFactorAuthenticator
	{
		public string GenerateOtp(string secretKey, int digits = 6, int timeStep = 30)
		{
			throw new NotImplementedException();
		}

		public string GenerateQrCode()
		{
			throw new NotImplementedException();
		}

		public bool VerifyOtpCode(int otpCode)
		{
			throw new NotImplementedException();
		}
	}
}
