using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class AccountGame
{
    [Required(ErrorMessage = "AccountID is required")]
    public int AccountId { get; set; }

    [Required(ErrorMessage = "GameID is required")]
    public int GameId { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}