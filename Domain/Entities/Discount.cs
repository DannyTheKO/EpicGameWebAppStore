using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Discount
{
    public int DiscountId { get; set; }

    public int? GameId { get; set; }

    public decimal? Percent { get; set; }

    public string Code { get; set; } = null!;
    public DateTime? StartOn { get; set; }

    public DateTime? EndOn { get; set; }

    [JsonIgnore]
    public virtual Game? Game { get; set; }
}