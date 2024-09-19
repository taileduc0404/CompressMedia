
namespace CompressMedia.DTOs
{
    public class BlobDto
    {
        public string? BlobId { get; set; }
        public string? BlobName { get; set; }
        public IFormFile? Data { get; set; }
        public DateTime UploadedDate { get; set; }
        public string? CompressionTime { get; set; }
        public string? ContentType { get; set; }
        public int ContainerId { get; set; }
        public string? Status { get; set; }
        public double Size { get; set; }
        public bool IsFps { get; set; }
        public bool IsResolution { get; set; }
        public bool IsBitrateVideo { get; set; }
    }
}
