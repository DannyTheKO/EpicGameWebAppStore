using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Accountgame
{
    public int AccountId { get; set; }

    public int GameId { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
