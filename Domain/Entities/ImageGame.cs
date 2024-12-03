using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Domain.Entities;

public class ImageGame
{
	public int ImageGameId { get; set; }

    [Required(ErrorMessage = "Game id is required")]
    public int GameId { get; set; }

    [Required(ErrorMessage = ("ImageType is required"))]
    public string ImageType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public DateTime? CreatedOn { get; set; }
    
    [JsonIgnore]
    public virtual Game Game { get; set; } = null!;
}
