using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class AccountGame
{
	public int AccountGameId { get; set; }

    [Required(ErrorMessage = "AccountID is required")]
    public int AccountId { get; set; }

    [Required(ErrorMessage = "GameID is required")]
    public int GameId { get; set; }

    public DateTime? DateAdded { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;

    [JsonIgnore]
    public virtual Game Game { get; set; } = null!;
}