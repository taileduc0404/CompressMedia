namespace CompressMedia.DTOs
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionDescription { get; set; }
        public int RoleId { get; set; }
        public bool IsSelected { get; set; }
    }
}
