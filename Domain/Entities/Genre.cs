using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
