using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
    public class BlobService : IBlobService
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageDbContext _storageContext;

        public BlobService(ApplicationDbContext context, BlobStorageDbContext storageContext)
        {
            _context = context;
            _storageContext = storageContext;
        }

        public async Task<bool> CreateBlobAsync(BlobDto blobDto)
        {

            if (blobDto == null)
            {
                return false;
            }

            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                await blobDto.Data?.CopyToAsync(memoryStream)!;
                data = memoryStream.ToArray();
            }

            BlobData blobData = new BlobData
            {
                Data = data
            };

            await _storageContext.BlobData.InsertOneAsync(blobData);
          

            Blob blob = new Blob
            {
                BlobName = blobDto.Data.FileName,
                ContainerId = blobDto.ContainerId,
                MetaData = new BlobMetadata
                {
                    BlobName= blobDto.Data?.FileName,
                    DataType =blobDto.Data?.ContentType,
                    Description = blobDto.Data?.FileName,
                    UploadedDate = DateTime.Now,  
                }
            };


            await _context.blobs.AddAsync(blob);
            await _context.SaveChangesAsync();

            return true;
        }

        public Task DeleteBlobAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetBlobContentAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetBlobMetadataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Blob>> GetListBlobAsync(int containerId)
        {
            return await _context.blobs.Where(b => b.ContainerId == containerId).ToListAsync();
        }

        public Task UpdateBlobAsync()
        {
            throw new NotImplementedException();
        }
    }
}
