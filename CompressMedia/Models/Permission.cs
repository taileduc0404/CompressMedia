namespace CompressMedia.Models
{
	public class Permission
	{
		public int PermissionId { get; set; }
		public string? PermissionName { get; set; }
		public string? PermissionDescription { get; set; }
		public virtual List<UserPermission>? UserPermissions { get; set; }
		public virtual List<RolePermission>? RolePermissions { get; set; }
		public virtual List<Role>? Roles { get; set; }
	}
}
