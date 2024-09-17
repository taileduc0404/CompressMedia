using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CompressMedia.Repositories
{
	public class BlobContainerService : IBlobContainerService
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthService _authService;

		public BlobContainerService(ApplicationDbContext context, IAuthService authService)
		{
			_context = context;
			_authService = authService;
		}

		public async Task<bool> DeleteAsync(string name)
		{
			BlobContainer? container = await _context.blobContainers.FirstOrDefaultAsync(c => c.ContainerName == name);
			if (container is null)
			{
				return false;
			}

			_context.blobContainers.Remove(container);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<ICollection<BlobContainer>> GetAsync()
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);
			return await _context.blobContainers.Where(c => c.UserId == user!.UserId).ToListAsync();

		}

		public async Task<bool> SaveAsync(ContainerDto containerDto)
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);
			if (userInfo == null)
			{
				return false;
			}
			BlobContainer container = new BlobContainer
			{
				ContainerName = containerDto.ContainerName!,
				UserId = user!.UserId,
			};

			await _context.blobContainers.AddAsync(container);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
