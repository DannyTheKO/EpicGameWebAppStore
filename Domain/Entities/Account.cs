using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Account
{
	public int AccountId { get; set; }

	[Required(ErrorMessage = "Username is required.")]
	public string? Username { get; set; }

	[Required(ErrorMessage = "Password is required.")]
	public string? Password { get; set; }

	public string? Email { get; set; }

	public string? IsAdmin { get; set; }

	public DateTime? CreatedOn { get; set; }

	public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}