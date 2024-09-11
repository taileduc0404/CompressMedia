namespace CompressMedia.Models
{
	public class Media
	{
		public int MediaId { get; set; }
		public string? MediaPath { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string? MediaType { get; set; }
		public long Size { get; set; }
		public string? Status { get; set; } // Original(nguyên bản), Compressing(đang xử lý nén), Compressed(đã nén xong)
		public string? UserId { get; set; }
		public virtual User? User { get; set; }
	}
}
