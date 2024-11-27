using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class DiscountFormModel
	{
		[Required(ErrorMessage = "EndOn is required")]
		public DateTime EndOn { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public DateTime StartOn { get; set; }

		[Required(ErrorMessage = "Code is required")]
		public string Code { get; set; }

		[Required(ErrorMessage = "Is this Percent?")]
		public decimal Percent { get; set; }

		[Required(ErrorMessage = "Game ID Id is required")]
		public int GameID { get; set; }
	}
}
