using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class CartFormModel
	{
		[Required(ErrorMessage = "Account ID is required")]
		public int AccountId { get; set; }

		[Required(ErrorMessage = "PaymentMethod ID is required")]
		public int PaymentMethodId { get; set; }
	}
}
