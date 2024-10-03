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
	public class BlobService : IBlobService
	{
		private readonly ApplicationDbContext _context;
		private readonly BlobStorageDbContext _storageContext;
		private readonly IGridFSBucket _gridFSBucket;

		public BlobService(ApplicationDbContext context, BlobStorageDbContext storageContext)
		{
			_context = context;
			_storageContext = storageContext;
			_gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
		}

		/// <summary>
		/// Get danh sách blob
		/// </summary>
		/// <param name="containerId"></param>
		/// <returns></returns>
		public async Task<ICollection<Blob>> GetListBlobAsync(int containerId)
		{
			return await _context.blobs.Where(b => b.ContainerId == containerId).ToListAsync();
		}

		/// <summary>
		/// Tạo 1 blob
		/// </summary>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		public async Task<bool> CreateBlobAsync(BlobDto blobDto)
		{
			if (blobDto.Data == null || blobDto.Data?.Length == 0)
			{
				return false;
			}

			var metadata = new BsonDocument
			{
				{"filename", blobDto.Data?.FileName },
				{"contentType", blobDto.Data?.ContentType },
				{"length",blobDto.Data?.Length},
				{"uploadDate", DateTime.Now }
			};

			using (Stream stream = blobDto.Data!.OpenReadStream())
			{
				var fileId = await _gridFSBucket.UploadFromStreamAsync(blobDto.Data.FileName, stream, new GridFSUploadOptions
				{
					Metadata = metadata
				});

				Blob newBlob = new Blob
				{
					BlobId = fileId.ToString(),
					ContainerId = blobDto.ContainerId,
					BlobName = blobDto.Data.FileName,
					Status = "Original",
					ContentType = blobDto.Data.ContentType,
					Size = blobDto.Data.Length,
					UploadDate = blobDto.UploadedDate,
				};
				await _context.blobs.AddAsync(newBlob);
				await _context.SaveChangesAsync();
			}

			return true;
		}

		/// <summary>
		/// Xóa 1 blob
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<string> DeleteBlobAsync(string blobId)
		{
			if (blobId is null)
			{
				return "null";
			}
			var oldBlob = await _context.blobs.FirstOrDefaultAsync(b => b.BlobId == blobId);
			if (oldBlob != null)
			{
				_context.blobs.Remove(oldBlob);
				await _context.SaveChangesAsync();
			}
			else
			{
				return "notfound";
			}

			var oldFileId = new ObjectId(blobId);
			var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, oldFileId);
			var oldFileEntry = await _gridFSBucket.FindAsync(filter);
			var oldFile = oldFileEntry.FirstOrDefault();
			if (oldFile != null)
			{
				await _gridFSBucket.DeleteAsync(oldFile.Id);
			}
			return "success";
		}

		/// <summary>
		/// Đọc dữ liệu blob vào stream để hiển thị vidoe lên
		/// </summary>
		/// <param name="blobId"></param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public async Task<Stream> GetBlobStreamAsync(string blobId)
		{
			var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, ObjectId.Parse(blobId));
			var searchResult = await _gridFSBucket.FindAsync(filter);
			var fileEntry = searchResult.FirstOrDefault();

			if (fileEntry != null)
			{
				var contentType = fileEntry.Metadata.GetValue("contentType").AsString;

				if (contentType.StartsWith("video/") || contentType.StartsWith("image/"))
				{
					var stream = await _gridFSBucket.OpenDownloadStreamAsync(fileEntry.Id);
					return stream;
				}
				else
				{
					throw new InvalidOperationException("Unsupported content type");
				}
			}
			throw new FileNotFoundException("Blob not found");
		}
	}
}