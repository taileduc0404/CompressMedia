
using CompressMedia.DTOs;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IAuthService
	{
		Task<string> Register(RegisterDto dto);
		Task<string> Login(LoginDto dto);
		void Logout();
		bool IsUserAuthenticated();
		string EncodeStringToBase64(LoginDto dto);
		void SetLoginCookie(string base64LoginInfo);
		string GetLoginInfoFromCookie();
		string DecodeFromBase64(string base64EncodedData);
		Task<string> GenerateQrCode(RegisterDto registerDto);
		Task<string> SendQrCodeViaEmail(RegisterDto registerDto);
		Task<string> SendOtpViaEmail(string secretKey, string fullName, string to);
		bool VerifyOtp(LoginDto loginDto);

	}
}
