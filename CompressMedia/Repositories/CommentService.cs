using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
	public class CommentService : ICommentService
	{
		private readonly ApplicationDbContext _context;

		public CommentService(ApplicationDbContext context) => _context = context;

		public async Task<string> CreateComment(string userId, CommentDto commentDto)
		{
			if (commentDto is null || userId is null)
				return null!;

			await _context.Comments.AddAsync(new Comment
			{
				UserId = userId,
				BlobId = commentDto.BlobId,
				Content = commentDto.Content,
				CreatedDate = commentDto.CreatedDate,
				ParentComment = 0
			});
			await _context.SaveChangesAsync();
			return "ok";
		}

		public async Task<List<Comment>> GetAllComment(string userId, string blobId)
			=> await _context.Comments.Include(x => x.User).Where(x => x.BlobId == blobId).ToListAsync() ?? new List<Comment>();

		public async Task<string> ReplyComment(int commentId, string userId, CommentDto commentDto)
		{
			if (commentDto is null || userId is null)
				return null!;
			Comment? comment = await _context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);

			await _context.Comments.AddAsync(new Comment
			{
				UserId = userId,
				BlobId = comment!.BlobId,
				Content = commentDto.Content,
				CreatedDate = commentDto.CreatedDate,
				ParentComment = commentId
			});
			await _context.SaveChangesAsync();
			return "ok";
		}
	}
}
