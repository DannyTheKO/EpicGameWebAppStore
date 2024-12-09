using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class ImageFormModel
	{
		[Required(ErrorMessage = "Game is required")]
		public int GameId { get; set; }

		[Required(ErrorMessage = "ImageType is required")]
		public string ImageType { get; set; }
        public IFormFile imageFile { get; set; }

    }
}
