using System;
using System.Collections.Generic;

namespace EpicGameWebAppStore.Temp;

public partial class Imagegame
{
    public int ImageGameId { get; set; }

    public int GameId { get; set; }

    public string ImageType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Game Game { get; set; } = null!;
}
