using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class GenreFormModel
	{
		[Required(ErrorMessage = "Require a Name for this Genre")]
		public string Name { get; set; }
	}
}
