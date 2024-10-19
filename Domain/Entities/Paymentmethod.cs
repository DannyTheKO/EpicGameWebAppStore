using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Paymentmethod
{
    public int PaymentMethodId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
