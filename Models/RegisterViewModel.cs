using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required.")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")] 
		public string ConfirmPassword { get; set; }

	}
}
