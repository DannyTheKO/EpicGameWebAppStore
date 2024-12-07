using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class DiscountFormModel
	{
		[Required(ErrorMessage = "EndOn is required")]
		[DataType(DataType.DateTime)]
		public DateTime EndOn { get; set; }

		[Required(ErrorMessage = "StartOn is required")]
		[DataType(DataType.DateTime)]
		public DateTime StartOn { get; set; }

		[Required(ErrorMessage = "Code is required")]
		[StringLength(45)]
		public string Code { get; set; }

		[Required(ErrorMessage = "Percent is required")]
		[Range(0, 100, ErrorMessage = "Percent must be between 0 and 100")]
		public decimal Percent { get; set; }

		[Required(ErrorMessage = "Game ID is required")]
		public int GameID { get; set; }
	}
}
