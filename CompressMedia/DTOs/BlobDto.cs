
using CompressMedia.Enums;

namespace CompressMedia.DTOs
{
	public class BlobDto
	{
		public string? BlobId { get; set; }
		public string? BlobName { get; set; }
		public string? Author { get; set; }
		public IFormFile? Data { get; set; }
		public DateTime UploadedDate { get; set; } = DateTime.Now;
		public string? CompressionTime { get; set; }
		public string? ContentType { get; set; }
		public int LikeCount { get; set; }
		public int CommentCount { get; set; }
		public int ContainerId { get; set; }
		public string? Status { get; set; }
		public double Size { get; set; }
		public bool IsFps { get; set; }
		public bool IsResolution { get; set; }
		public bool IsBitrateVideo { get; set; }
		public ImageManipulationMode? ImageResizeMode { get; set; }
		public ImageZoomMode? ImageZoomMode { get; set; }
		public ImageAspectRatioMode? ImageAspectRatioMode { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
