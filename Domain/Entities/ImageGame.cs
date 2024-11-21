﻿using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ImageGame
{
    public int ImageId { get; set; }

    public int GameId { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public DateTime? CreateAt { get; set; }
    [JsonIgnore]
    public virtual Game Game { get; set; } = null!;
}
