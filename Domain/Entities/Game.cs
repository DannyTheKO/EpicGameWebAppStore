using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Game
{
    public int GameId { get; set; }

    public int? PublisherId { get; set; }

    public int? GenreId { get; set; }

    public int ImageId { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public string? Author { get; set; }

    public DateTime? Release { get; set; }

    public decimal? Rating { get; set; }

    public string? Description { get; set; }

    [JsonIgnore] 
    public virtual ICollection<AccountGame> AccountGames { get; set; } = new List<AccountGame>();

    [JsonIgnore]
    public virtual ICollection<Cartdetail> CartDetails { get; set; } = new List<Cartdetail>();
    
    [JsonIgnore]
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<ImageGame> Images { get; set; } = new List<ImageGame>();
    public virtual Publisher? Publisher { get; set; }
}