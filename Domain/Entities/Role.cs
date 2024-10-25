using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
