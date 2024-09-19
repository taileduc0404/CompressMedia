namespace CompressMedia.Models
{
	public class Blob
	{
		public string BlobId { get; set; }
		public string BlobName { get; set; }
		//public string? BlobDataId { get; set; }
		public int ContainerId { get; set; }
		public BlobContainer? BlobContainer { get; set; }
		public string ContentType { get; set; }
		public string? Status { get; set; }
	}
}
