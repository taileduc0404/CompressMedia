namespace CompressMedia.DTOs
{
    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
    }
}
