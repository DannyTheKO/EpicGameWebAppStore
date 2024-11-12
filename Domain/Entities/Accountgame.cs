namespace Domain.Entities;

public class AccountGame
{
    public int AccountId { get; set; }

    public int GameId { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}