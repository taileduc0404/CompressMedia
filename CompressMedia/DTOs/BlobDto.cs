using System.ComponentModel.DataAnnotations;

namespace CompressMedia.DTOs
{
    public class BlobDto
    {
        public string? BlobId { get; set; }
        [Required]
        public string? BlobName { get; set; }
        [Required]
        public IFormFile? Data { get; set; }
        public DateTime UploadedDate { get; set; }

        public string? ContentType { get; set; }

        [Required]
        public int ContainerId { get; set; }

        public string? Status { get; set; }

        public bool IsFps { get; set; }
        public bool IsResolution { get; set; }
        public bool IsBitrateVideo { get; set; }
    }
}
