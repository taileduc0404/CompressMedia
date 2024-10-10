namespace CompressMedia.Models
{
	public class Role
	{
		public int RoleId { get; set; }
		public Guid? TenantId { get; set; }
		public Tenant? Tenant { get; set; }
		public string? RoleName { get; set; }
		public virtual ICollection<User>? Users { get; set; }
		public virtual ICollection<Permission>? Permissions { get; set; }
		public virtual ICollection<RolePermission>? RolePermissions { get; set; }
	}
}
