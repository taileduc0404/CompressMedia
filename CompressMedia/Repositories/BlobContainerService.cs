using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace CompressMedia.Repositories
{
	public class BlobContainerService : IBlobContainerService
	{
		private readonly ApplicationDbContext _context;
		private readonly IUserService _userService;
		private readonly BlobStorageDbContext _storageContext;
		private readonly IGridFSBucket _gridFSBucket;

		public BlobContainerService(ApplicationDbContext context, BlobStorageDbContext storageContext, IUserService userService)
		{
			_context = context;
			_storageContext = storageContext;
			_gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
			_userService = userService;
		}

		/// <summary>
		/// Xóa container
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int containerId)
		{
			BlobContainer? container = await _context.BlobContainers.FirstOrDefaultAsync(c => c.ContainerId == containerId);
			List<Blob> blobs = await _context.Blobs.Where(x => x.ContainerId == containerId).ToListAsync();

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

			_context.BlobContainers.Remove(container);
			await _context.SaveChangesAsync();
			return true;
		}

		/// <summary>
		/// Get danh sách container
		/// </summary>
		/// <returns></returns>
		public async Task<ICollection<BlobContainer>> GetAsync()
		{
			string username = _userService.GetUserNameLoggedIn();
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

			return await _context.BlobContainers.Where(c => c.UserId == user!.UserId).ToListAsync();
		}

		/// <summary>
		/// Lấy danh sách container của tenant
		/// </summary>
		/// <param name="tenantId"></param>
		/// <returns></returns>
		public async Task<ICollection<BlobContainer>> GetAsync(Guid? tenantId)
		{
			string username = _userService.GetUserNameLoggedIn();
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

			if (user!.TenantId is null)
			{
				return await _context.BlobContainers.Include(x => x.Tenant).ToListAsync();
			}
			return await _context.BlobContainers.Include(x => x.Tenant).Where(c => c.TenantId == tenantId).ToListAsync();
		}

		/// <summary>
		/// Lưu container
		/// </summary>
		/// <param name="containerDto"></param>
		/// <returns></returns>
		public async Task<string> SaveAsync(ContainerDto containerDto)
		{
			string username = _userService.GetUserNameLoggedIn();
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

			if (user == null)
			{
				return "null";
			}

			BlobContainer? findContainer = await _context.BlobContainers.Where(u => u.TenantId == user!.TenantId)
													.SingleOrDefaultAsync(x => x.ContainerName == containerDto.ContainerName);
			if (findContainer != null)
			{
				return "exist";
			}

			if (containerDto is not null)
			{
				BlobContainer container = new BlobContainer
				{
					ContainerName = containerDto.ContainerName!,
					TenantId = user!.TenantId,
					UserId = user!.UserId,
				};
				await _context.BlobContainers.AddAsync(container);
				await _context.SaveChangesAsync();
			}

			return "true";
		}
	}
}
