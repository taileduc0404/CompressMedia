namespace CompressMedia.DTOs
{
	public class ReportDto
	{
		public string? MediaId { get; set; }
		public string? MediaName { get; set; }
		public string? TenantName { get; set; }
		public int ReportCount { get; set; }
		public DateTime FirstReportDate { get; set; }
	}
}
