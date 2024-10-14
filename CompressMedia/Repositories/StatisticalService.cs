using CompressMedia.Data;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
    public class StatisticalService : IStatisticalService
    {
        private readonly ApplicationDbContext _context;

        public StatisticalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blob>> Get10MediaWithTheMostComments(Guid? tenantId)
        {
            return await _context.Blobs
                .Include(c => c.Comments)!
                .ThenInclude(comment => comment.User)
                .Where(x => x.Comments!.Count() > 0)
                .OrderByDescending(x => x.Comments!.Count)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Blob>> Get10MediaWithTheMostLikes(Guid? tenantId)
        {
            var blobLikes = await _context.Likes
                .Where(x => x.Blob!.TenantId == tenantId)
                .GroupBy(x => x.BlobId)
                .Where(group => group.Count() > 0)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .Select(group => group.FirstOrDefault()!.Blob)
                .ToListAsync();

            var blobWithUsers = await _context.Blobs
                .Include(b => b.User)
                .Where(b => blobLikes.Contains(b))
                .ToListAsync();

            return blobWithUsers;
        }

    }
}
