using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Paymentmethod
{
    public int PaymentMethodId { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}