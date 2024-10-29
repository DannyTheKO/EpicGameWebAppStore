using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public bool IsAdmin { get; set; }  // Changed from string to bool

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
