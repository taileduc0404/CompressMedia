using System.ComponentModel.DataAnnotations;

namespace CompressMedia.DTOs
{
    public class BlobDto
    {
        //public int BlobId { get; set; }
        [Required]
        public string BlobName { get; set; }
        [Required]
        public IFormFile? Data { get; set; }
        [Required]
        public MetaDataDto? MetaData { get; set; }
        [Required]
        public int ContainerId { get; set; }
        public string Status { get; set; }
    }
}
