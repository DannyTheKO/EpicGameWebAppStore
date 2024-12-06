using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class ForgotPasswordForm
	{
		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; } = null!;

	}
}
