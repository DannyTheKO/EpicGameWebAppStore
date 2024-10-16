﻿using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Domain.Entities;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
