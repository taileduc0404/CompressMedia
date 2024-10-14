namespace CompressMedia.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string? MediaId { get; set; }
        public string? UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ReportDate { get; set; }
        public Tenant? Tenant { get; set; }
        public Blob? Blob { get; set; }
        public User? User { get; set; }
    }
}
