using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class AccountFormModel
	{
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Is this Account Active ?")]
		public string IsActive { get; set; }

		[Required(ErrorMessage = "Role Id is required")]
		public int RoleId { get; set; }
	}
}
