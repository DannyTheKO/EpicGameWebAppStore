
using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
	public class AccountGameFormModel
	{
		[Required(ErrorMessage = "AccountID is required")]
		public int AccountId { get; set; }
		
		[Required(ErrorMessage = "GameID is required")]
		public int GameId { get; set; }
	}
}
