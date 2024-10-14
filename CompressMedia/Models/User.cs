namespace CompressMedia.Models
{
	public class User
	{
		public string? UserId { get; set; }
		public Guid? TenantId { get; set; }
		public Tenant? Tenant { get; set; }
		public int? RoleId { get; set; }
		public virtual Role? Role { get; set; }
		public string? Username { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? PasswordHash { get; set; }
		public string? SecretKey { get; set; }
		public virtual ICollection<UserPermission>? UserPermissions { get; set; }
		public virtual ICollection<Blob>? Blobs { get; set; }
		public virtual ICollection<BlobContainer>? Containers { get; set; }
		public virtual ICollection<Comment>? Comments { get; set; }
	}
}
