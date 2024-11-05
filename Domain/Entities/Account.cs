using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Domain.Entities;

public class Account
{
    public int AccountId { get; set; }

    public int? RoleId { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }

    public string? Email { get; set; }

    [Required(ErrorMessage = "Is this Account Active ?")]
    public string? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Role? Role { get; set; }
}