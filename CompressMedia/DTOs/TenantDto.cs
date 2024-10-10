namespace CompressMedia.DTOs
{
	public class TenantDto
	{
		public Guid TenantId { get; set; } = Guid.NewGuid();
		public string? TenantName { get; set; }
		public string? CompanyName { get; set; }
		public RegisterDto? RegisterDto { get; set; }
	}
}
