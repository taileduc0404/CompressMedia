using CompressMedia.CustomAuthentication;
using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CompressMedia.Services
{
	public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthService(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_applicationDbContext = applicationDbContext;
			_httpContextAccessor = httpContextAccessor;
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

				string userInfoEncode = EncodeStringToBase64(dto);

				SetLoginCookie(userInfoEncode);

				return "success";
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
				if (CheckUsernameAndEmail(dto))
				{
					return "usernameExist";
				}

				if (dto is null)
				{
					return "null";
				}

				var user = new User
				{
					UserId = Guid.NewGuid().ToString(),
					Username = dto.Username,
					FirstName = dto.FirstName,
					LastName = dto.LastName,
					Email = dto.Email,
					PasswordHash = PasswordHasher.HashPassword(dto.Password!).ToString()
				};

				_applicationDbContext.users.Add(user);
				await _applicationDbContext.SaveChangesAsync();
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
				Expires = DateTimeOffset.UtcNow.AddMinutes(30)
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
	}
}
