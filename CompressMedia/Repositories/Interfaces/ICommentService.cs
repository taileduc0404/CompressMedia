using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllComment(string userId, string blobId);
        Task<string> CreateComment(string userId, CommentDto commentDto);
        Task<string> ReplyComment(int commentId, string userId, CommentDto commentDto);
    }
}
