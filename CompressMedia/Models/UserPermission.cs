namespace CompressMedia.Models
{
    public class UserPermission
    {
        public string? UserId { get; set; }
        public User? User { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }

}
