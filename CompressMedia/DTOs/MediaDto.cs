namespace CompressMedia.DTOs
{
    public class MediaDto
    {
        public int MediaId { get; set; }
        public IFormFile? Media { get; set; }
        public string? MediaPath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? MediaType { get; set; }
        public string? Status { get; set; }
        public string? CompressDuration { get; set; }
		public string? UserId { get; set; }
        public double Size { get; set; }

        // Compress Options:
        public bool IsFps { get; set; } 
        public bool IsResolution { get; set; }
        public bool IsBitrateVideo { get; set; } 
    }
}
