using System.ComponentModel.DataAnnotations;

namespace CompressMedia.DTOs
{
    public class UserDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 10 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "FirstName must contain only letters and spaces.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "LastName must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "LastName must contain only letters and spaces.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(320, MinimumLength = 11, ErrorMessage = "Email must be than 10 character")]
        public string? Email { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,30}$",
          ErrorMessage = "Password must be between 6 and 30 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? OldPassword { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,30}$",
          ErrorMessage = "Password must be between 6 and 30 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? NewPassword { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,30}$",
          ErrorMessage = "Password must be between 6 and 30 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string? ConfirmNewPassword { get; set; }

        public string? SecretKey { get; set; }

    }
}
