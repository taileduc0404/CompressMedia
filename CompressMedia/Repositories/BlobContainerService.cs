using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;

namespace CompressMedia.Repositories
{
	public class BlobContainerService : IBlobContainerService
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthService _authService;
		private readonly BlobStorageDbContext _storageContext;
		private readonly IGridFSBucket _gridFSBucket;

		public BlobContainerService(ApplicationDbContext context, IAuthService authService, BlobStorageDbContext storageContext)
		{
			_context = context;
			_authService = authService;
			_storageContext = storageContext;
			_gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
		}

		/// <summary>
		/// Xóa container
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int containerId)
		{
			BlobContainer? container = await _context.blobContainers.FirstOrDefaultAsync(c => c.ContainerId == containerId);
			List<Blob> blobs = await _context.blobs.Where(x => x.ContainerId == containerId).ToListAsync();
			if (container is null)
			{
				return false;
			}

			foreach (var blob in blobs)
			{
				var oldFileId = new ObjectId(blob.BlobId);
				var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, oldFileId);
				var oldFileEntry = await _gridFSBucket.FindAsync(filter);
				var oldFile = oldFileEntry.FirstOrDefault();
				if (oldFile != null)
				{
					await _gridFSBucket.DeleteAsync(oldFile.Id);
				}
			}

			_context.blobContainers.Remove(container);
			await _context.SaveChangesAsync();
			return true;
		}

		/// <summary>
		/// Get danh sách container
		/// </summary>
		/// <returns></returns>
		public async Task<ICollection<BlobContainer>> GetAsync()
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);

			return await _context.blobContainers.Where(c => c.UserId == user!.UserId).ToListAsync();
		}

		/// <summary>
		/// Lưu container
		/// </summary>
		/// <param name="containerDto"></param>
		/// <returns></returns>
		public async Task<string> SaveAsync(ContainerDto containerDto)
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);

			if (userInfo == null)
			{
				return "null";
			}

			BlobContainer? findContainer = await _context.blobContainers.Where(u => u.User!.Username == user!.Username)
													.SingleOrDefaultAsync(x => x.ContainerName == containerDto.ContainerName);
			if (findContainer != null)
			{
				return "exist";
			}

			BlobContainer container = new BlobContainer
			{
				ContainerName = containerDto.ContainerName!,
				UserId = user!.UserId,
			};

			await _context.blobContainers.AddAsync(container);
			await _context.SaveChangesAsync();
			return "true";
		}
	}
}
