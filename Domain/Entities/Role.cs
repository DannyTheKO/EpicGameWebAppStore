namespace Domain.Entities;

public class Role
{
	public int RoleId { get; set; }

	public string? Name { get; set; }

	public List<string>? Permission { get; set; }

	public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}