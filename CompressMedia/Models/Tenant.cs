namespace CompressMedia.Models
{
	public class Tenant
	{
		public Guid TenantId { get; set; }
		public string? TenantName { get; set; }
		public string? CompanyName { get; set; }
		public virtual ICollection<User>? Users { get; set; }
		public virtual ICollection<Role>? Roles { get; set; }
		public virtual ICollection<BlobContainer>? BlobContainers { get; set; }
		public virtual ICollection<Blob>? Blobs { get; set; }
	}
}
