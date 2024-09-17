namespace CompressMedia.Models
{
	public class BlobMetadata
	{
        public int MetadataId { get; set; }
        public string? BlobName { get; set; }
		public DateTime UploadedDate { get; set; }
		public string? DataType { get; set; }
		public string? Description { get; set; }
        public Blob? Blob { get; set; }
    }
}
