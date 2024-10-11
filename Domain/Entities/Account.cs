using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? IsAdmin { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
