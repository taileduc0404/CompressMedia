using System.ComponentModel.DataAnnotations;

namespace CompressMedia.DTOs
{
	public class LoginDto
	{
		[Required(ErrorMessage = "Username is required.")]
		[StringLength(10, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 10 characters.")]
		public string? Username { get; set; }

		[Required(ErrorMessage = "Username is required")]
		public string? Password { get; set; }
	}
}
