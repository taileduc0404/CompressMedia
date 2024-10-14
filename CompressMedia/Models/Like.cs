namespace CompressMedia.Models
{
	public class Like
	{
		public int Id { get; set; }
		public string? BlobId { get; set; }
		public string? UserId { get; set; }
		public DateTime LikedAt { get; set; }
		public Blob? Blob { get; set; }
		public User? User { get; set; }
	}
}
