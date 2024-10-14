using CompressMedia.Models;

namespace CompressMedia.DTOs
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public Guid? TenantId { get; set; }
        public string? TenantName { get; set; }
        public Tenant? Tenant { get; set; }
        public string? RoleName { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
