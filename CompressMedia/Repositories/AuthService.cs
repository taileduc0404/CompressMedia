using CompressMedia.CustomAuthentication;
using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Helpers;
using CompressMedia.Models;
using CompressMedia.Repositories;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace CompressMedia.Services
{
	public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_applicationDbContext = applicationDbContext;
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}
		private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

		/// <summary>
		/// Đăng nhập
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<string> Login(LoginDto dto)
		{
			try
			{
				var user = await _applicationDbContext.users.SingleOrDefaultAsync(u => u.Username == dto.Username);

				if (user is null)
				{
					return "u";
				}
				if (!PasswordHasher.VerifyPassword(dto.Password!, user.PasswordHash!))
				{
					return "p";
				}

				return user.Username!;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Kiểm tra xem người dùng đăng nhập chưa
		/// </summary>
		/// <returns></returns>
		public bool IsUserAuthenticated()
		{
			string cookie = GetLoginInfoFromCookie();

			if (cookie == null)
			{
				return false;
			}

			string cookieDecode = DecodeFromBase64(cookie);

			var loginInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);

			var user = _applicationDbContext.users.SingleOrDefault(u => u.Username == loginInfo.Username);

			return user is not null;

		}

		/// <summary>
		/// Đăng ký tài khoản
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<string> Register(RegisterDto dto)
		{
			try
			{
				if (dto is null)
				{
					throw new ArgumentNullException(nameof(dto), "RegisterDto cannot be null");
				}

				if (string.IsNullOrWhiteSpace(dto.Password))
				{
					throw new ArgumentException("Password cannot be null or whitespace", nameof(dto.Password));
				}

				if (CheckUsernameAndEmail(dto))
				{
					return "usernameExist";
				}

				var user = new User
				{
					UserId = Guid.NewGuid().ToString(),
					Username = dto.Username,
					FirstName = dto.FirstName,
					LastName = dto.LastName,
					Email = dto.Email,
					PasswordHash = PasswordHasher.Hash(dto.Password).ToString(),
					SecretKey = Guid.NewGuid().ToString()
				};

				_applicationDbContext.users.Add(user);
				await _applicationDbContext.SaveChangesAsync();
				// Gửi qr đính kèm trong mail
				await SendQrCodeViaEmail(dto);

				return "success";
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Kiểm tra xem username hoặc email có tồn tại chưa
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		private bool CheckUsernameAndEmail(RegisterDto dto)
		{
			return _applicationDbContext.users.Any(user => user.Username == dto.Username || user.Email == dto.Email);
		}

		/// <summary>
		/// Mã hóa thông tin người dùng sang base64
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		public string EncodeStringToBase64(LoginDto dto)
		{
			string dtoJson = JsonConvert.SerializeObject(dto);
			byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(dtoJson);
			return Convert.ToBase64String(bytesToEncode);
		}

		/// <summary>
		/// Lưu thông tin người dùng vào coookie
		/// </summary>
		/// <param name="base64LoginInfo"></param>
		public void SetLoginCookie(string base64LoginInfo)
		{
			CookieOptions options = new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = DateTimeOffset.UtcNow.AddMinutes(60)
			};

			HttpContext.Response.Cookies.Append("LoginInfo", base64LoginInfo, options);
		}

		/// <summary>
		/// Lấy thông tin người dùng từ cookie
		/// </summary>
		/// <returns></returns>
		public string GetLoginInfoFromCookie()
		{
			HttpContext.Request.Cookies.TryGetValue("LoginInfo", out string? base64LoginInfo);
			return base64LoginInfo!;
		}

		/// <summary>
		/// Giải mã cookie
		/// </summary>
		/// <param name="base64EncodedData"></param>
		/// <returns></returns>
		public string DecodeFromBase64(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		/// <summary>
		/// Đăng xuất
		/// </summary>
		public void Logout()
		{
			HttpContext.Response.Cookies.Delete("LoginInfo");
		}

		/// <summary>
		/// Tạo qr với thông tin người dùng 
		/// </summary>
		/// <param name="loginDto"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<string> GenerateQrCode(RegisterDto registerDto)
		{
			User? user = await _applicationDbContext.users.SingleOrDefaultAsync(u => u.Username == registerDto.Username);
			if (user == null) throw new Exception("User not found");

			string qrCodeData = $"{user.SecretKey}";

			byte[] secretByte = Encoding.UTF8.GetBytes(qrCodeData);

			TwoFactorAuthenticator twoFacAuth = new();
			SetupCode setupInfo = twoFacAuth.GenerateSetupCode(
				"CompressMedia",
				registerDto.Username!,
				secretByte,
				3
			);
			string qrCodeUrl = setupInfo.QrCodeSetupImageUrl;

			return qrCodeUrl;
		}

		/// <summary>
		/// Xác thực otp
		/// </summary>
		/// <param name="loginDto"></param>
		/// <returns></returns>
		public bool VerifyOtp(LoginDto loginDto)
		{
			TwoFactorAuthenticator twoFactorAuthenticator = new();
			User? user = _applicationDbContext.users.SingleOrDefault(u => u.Username == loginDto.Username);
			if (user == null) throw new Exception("User not found");

			string qrCodeData = $"{user.SecretKey}";
			byte[] qrCodeDataBytes = Encoding.UTF8.GetBytes(qrCodeData!);
			if (loginDto.OtpCode == null)
			{
				return false;
			}

			bool isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(qrCodeDataBytes, loginDto.OtpCode!, TimeSpan.FromSeconds(3));
			if (isValid)
			{

				string userInfoEncode = EncodeStringToBase64(loginDto);
				SetLoginCookie(userInfoEncode);
				return true;
			}
			return false;
		}

		public async Task<string> SendQrCodeViaEmail(RegisterDto registerDto)
		{
			if (registerDto is null)
			{
				return null!;
			}
			User? user = await _applicationDbContext.users.SingleOrDefaultAsync(u => u.Username == registerDto.Username);
			if (user == null) throw new Exception("User not found");

			string qrCodeData = $"{user.Username}:{user.SecretKey}";
			string qrCode = await GenerateQrCode(registerDto);
			EmailProviderFactory emailProviderFactory = new EmailProviderFactory(_configuration);

			IEmailProvider emailProvider = emailProviderFactory.CreateEmailProvider();
			await emailProvider.SendEmailAsync(registerDto.FirstName + " " + registerDto.LastName, registerDto.Email!, qrCode);
			return "QR Code has send to your email";
		}

		public async Task<string> SendOtpViaEmail(string secretKey, string fullName, string to)
		{
			if (secretKey is null || fullName is null || to is null)
			{
				return null!;
			}

			TwoFactorAuthenticator twoFactorAuthenticator = new();
			byte[] userDataByte = Encoding.UTF8.GetBytes(secretKey);
			string otp = twoFactorAuthenticator.GenerateTotp(userDataByte, twoFactorAuthenticator.GetCurrentTimeStepNumber());
			EmailProviderFactory emailProviderFactory = new EmailProviderFactory(_configuration);

			IEmailProvider emailProvider = emailProviderFactory.CreateEmailProvider();
			await emailProvider.SendEmailAsync(fullName, to, otp);
			HttpContext.Session.SetString("QrCodeData", secretKey);
			return "OTP has send to your email";
		}
	}
}
