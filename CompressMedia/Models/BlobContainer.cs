namespace CompressMedia.Models
{
	public class BlobContainer
	{
		public int ContainerId { get; set; }
		public string ContainerName { get; set; }
		public string? UserId { get; set; }
		public User? User { get; set; }
		public virtual ICollection<Blob>? Blobs { get; set; }
	}
}
