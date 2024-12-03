using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Temp;

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

    public virtual ICollection<Imagegame> Imagegames { get; set; } = new List<Imagegame>();
}
