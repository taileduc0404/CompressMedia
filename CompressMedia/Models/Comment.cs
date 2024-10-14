namespace CompressMedia.Models
{
	public class Comment
	{
		public int CommentId { get; set; }
		public string? Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public string? BlobId { get; set; }
		public string? UserId { get; set; }
		public string? UserName { get; set; }
		public Blob? Blob { get; set; }
		public User? User { get; set; }
	}
}
