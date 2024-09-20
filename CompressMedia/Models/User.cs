using System.ComponentModel.DataAnnotations;

namespace CompressMedia.Models
{
    public class User
    {

        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecretKey { get; set; }
        //public bool IsActive { get; set; }
        //public Guid ActivevationCode { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
        public virtual ICollection<Media>? Medias { get; set; }
        public virtual ICollection<BlobContainer>? Containers { get; set; }
    }
}
