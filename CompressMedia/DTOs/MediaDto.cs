namespace CompressMedia.DTOs
{
	public class MediaDto
	{
		public IFormFile? Media { get; set; }
		public string? MediaPath { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		//public string? MediaType { get; set; }
		//public string? UserId { get; set; }
		public double Size { get; set; }
	}
}
