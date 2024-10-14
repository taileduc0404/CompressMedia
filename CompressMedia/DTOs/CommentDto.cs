using CompressMedia.Models;

namespace CompressMedia.DTOs
{
	public class CommentDto
	{
		//public int CommentId { get; set; }
		public string? Content { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string? BlobId { get; set; }
		public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
