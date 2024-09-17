namespace CompressMedia.Models
{
	public class Blob
	{
		public int BlobId { get; set; }
		public string BlobName { get; set; }
		public int? BlobDataId { get; set; }
		public int ContainerId { get; set; }
		public BlobContainer? BlobContainer { get; set; }
		public BlobContainer? Container { get; set; }
		public BlobMetadata? MetaData { get; set; }
		public string? Status { get; set; }
	}
}
