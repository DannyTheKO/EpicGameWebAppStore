using System.ComponentModel.DataAnnotations;
using EpicGameWebAppStore.Controllers;
using Mysqlx;

namespace EpicGameWebAppStore.Models
{
	public class GameFormModel
	{
		public int PublisherId { get; set; }

		public int GenreId { get; set; }

		[Required(ErrorMessage = "Title is required")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Price is required")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Author is required")]
		public string Author { get; set; }

		public DateTime? Release { get; set; }

		public decimal Rating { get; set; }

		public string Description { get; set; }
	}
}
