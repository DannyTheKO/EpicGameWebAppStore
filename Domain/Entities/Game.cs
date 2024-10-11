using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Game
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

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual Genre? Genre { get; set; }

    public virtual Publisher? Publisher { get; set; }
}
