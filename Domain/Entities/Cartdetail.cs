using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Cartdetail
{
    public int CartDetailId { get; set; }
    public int CartId { get; set; }
    public int GameId { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public decimal? Discount { get; set; }

    [JsonIgnore]
    public virtual Cart Cart { get; set; } = null!;
    
    [JsonIgnore]
    public virtual Game Game { get; set; } = null!;
}