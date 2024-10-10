namespace CompressMedia.Models
{
	public class Blob
	{
		public string? BlobId { get; set; }
		public Guid? TenantId { get; set; }
		public Tenant? Tenant { get; set; }
		public string? BlobName { get; set; }
		public double Size { get; set; }
		public string CompressionTime { get; set; } = "00:00:00";
		public int ContainerId { get; set; }
		public DateTime UploadDate { get; set; } = DateTime.Now;
		public BlobContainer? BlobContainer { get; set; }
		public string? ContentType { get; set; }
		public string? Status { get; set; }
	}
}
