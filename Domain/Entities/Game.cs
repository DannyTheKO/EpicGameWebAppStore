namespace Domain.Entities;

public class Game
{
	public int GameId { get; set; }

	public int? PublisherId { get; set; }

	public int? GenreId { get; set; }

	public string? Title { get; set; }

	public decimal? Price { get; set; }

	public string? Author { get; set; }

	public DateTime? Release { get; set; }

	public decimal? Rating { get; set; }

	public string? Description { get; set; }
}