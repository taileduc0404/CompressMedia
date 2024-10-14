using CompressMedia.Data;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
	public class LikeService : ILikeService
	{
		private readonly ApplicationDbContext _context;

		public LikeService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task LikeBlob(string blobId, string userId)
		{
			var existingLike = await _context.Likes
				.FirstOrDefaultAsync(l => l.BlobId == blobId && l.UserId == userId);

			if (existingLike == null)
			{
				var like = new Like
				{
					BlobId = blobId,
					UserId = userId,
					LikedAt = DateTime.Now
				};
				_context.Likes.Add(like);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> IsBlobLikedByUser(string blobId, string userId)
		{
			return await _context.Likes
				.AnyAsync(l => l.BlobId == blobId && l.UserId == userId);
		}

		public async Task<int> GetLikesCount(string blobId)
		{
			return await _context.Likes
				.CountAsync(l => l.BlobId == blobId);
		}

		public async Task<string> DeleteUserLike(string blobId, string userId)
		{
			bool isLike = await IsBlobLikedByUser(blobId, userId);
			Like? like = await _context.Likes.FirstOrDefaultAsync(l => l.BlobId == blobId && l.UserId == userId);
			if (!isLike || like is null)
			{
				return null!;
			}
			_context.Likes.Remove(like);
			await _context.SaveChangesAsync();
			return "ok";
		}
	}
}
