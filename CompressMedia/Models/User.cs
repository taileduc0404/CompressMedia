using System.ComponentModel.DataAnnotations;

namespace CompressMedia.Models
{
    public class User
    {
        
        public string UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 10 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? PasswordHash { get; set; }
        //public bool IsActive { get; set; }
        //public Guid ActivevationCode { get; set; }
        public virtual ICollection<Role>? Roles { get; set; }
        public virtual ICollection<Media>? Medias { get; set; }
    }
}
