using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class CartDetailFormModel
	{
		[Required(ErrorMessage = "CartID is required")]
		public int CartId { get; set; }

		[Required(ErrorMessage = "GameID is required")]
		public int GameId { get; set; }
		

		public decimal Discount { get; set; }
	}


}
