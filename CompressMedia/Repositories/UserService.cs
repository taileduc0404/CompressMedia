using CompressMedia.CustomAuthentication;
using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompressMedia.Repositories
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthService _authService;

		public UserService(ApplicationDbContext context, IAuthService authService)
		{
			_context = context;
			_authService = authService;
		}

		/// <summary>
		/// Lấy thông tin đnăg nhập
		/// </summary>
		/// <returns></returns>
		private LoginDto GetLoginInfo()
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			return userInfo;
		}

		/// <summary>
		/// Chỉnh sửa profile
		/// </summary>
		/// <param name="userDto"></param>
		/// <returns></returns>
		public async Task<User> EditProfile(UserDto userDto)
		{
			User user = await GetUserByName(userDto.Username!);

			LoginDto userInfo = GetLoginInfo();

			User? userUpdate = await _context.Users.FindAsync(user.UserId);

			if (string.IsNullOrWhiteSpace(userDto.OldPassword) || string.IsNullOrWhiteSpace(userDto.NewPassword) || string.IsNullOrWhiteSpace(userDto.ConfirmNewPassword))
			{
				userUpdate!.PasswordHash = user.PasswordHash;
			}

			if (!string.IsNullOrWhiteSpace(userDto.OldPassword) && !string.IsNullOrWhiteSpace(userDto.NewPassword) && !string.IsNullOrWhiteSpace(userDto.ConfirmNewPassword))
			{
				if (userDto.OldPassword == userInfo.Password && userDto.NewPassword == userDto.ConfirmNewPassword)
				{
					userUpdate!.PasswordHash = PasswordHasher.Hash(userDto.NewPassword);
				}
			}

			if (userUpdate != null)
			{
				userUpdate.Username = userDto.Username;
				userUpdate.FirstName = userDto.FirstName;
				userUpdate.LastName = userDto.LastName;
				userUpdate.Email = userDto.Email;
				userUpdate.PasswordHash = userUpdate!.PasswordHash;
				await _context.SaveChangesAsync();
			}
			return user;
		}

		/// <summary>
		/// Lấy danh sách user
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<User>> GetAllUser(Guid? tenantId)
		{
			bool isAuthen = _authService.IsUserAuthenticated();
			if (!isAuthen)
			{
				return null!;
			}

			IEnumerable<User> users;
			if (tenantId == null)
			{
				users = await _context.Users.ToListAsync();

			}
			else
			{
				users = await _context.Users.Where(x => x.TenantId == tenantId).ToListAsync();

			}

			return users;

		}

		/// <summary>
		/// Lấy ra 1 user bằng usernmae
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public async Task<User> GetUserByName(string username)
		{
			bool isAuthen = _authService.IsUserAuthenticated();
			if (!isAuthen)
			{
				return null!;
			}
			string cookie = _authService.GetLoginInfoFromCookie();

			string cookieDecode = _authService.DecodeFromBase64(cookie);

			LoginDto user = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);

			if (user == null)
			{
				return null!;
			}

			User? userInfo = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);

			if (userInfo == null)
			{
				return null!;
			}

			return userInfo;
		}

		/// <summary>
		/// Lấy username của user đang login
		/// </summary>
		/// <returns></returns>
		public string GetUserNameLoggedIn()
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			if (cookie != null)
			{
				string cookieDecode = _authService.DecodeFromBase64(cookie);
				User user = JsonConvert.DeserializeObject<User>(cookieDecode);
				return user?.Username ?? string.Empty;
			}

			return string.Empty;

		}
	}
}
