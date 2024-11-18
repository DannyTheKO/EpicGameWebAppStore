using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Account
{
    public int AccountId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    [Required(ErrorMessage = "Is this Account Active ?")]
    public string IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [JsonIgnore]
    public virtual Role? Role { get; set; }
}