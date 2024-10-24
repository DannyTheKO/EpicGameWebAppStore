using DataAccess;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
// Domain

// Infrastructure


namespace Infrastructure.Repository;

public class AccountRepository : IAccountRepository
{
	private readonly EpicGameDbContext _context;

	public AccountRepository(EpicGameDbContext context)
	{
		_context = context;
	}

	#region == Basic CRUD operation ==

	// ACTION: Create User into Database
	public async Task AddUserAsync(Account account)
	{
		_context.Accounts.Add(account);
		await _context.SaveChangesAsync();
	}

	// SELECT: Get all User from the Database 
	public async Task<IEnumerable<Account>> GetAllUserAsync()
	{
		return await _context.Accounts.ToListAsync();
	}

	// ACTION: Update User From the Database
	public async Task UpdateUserAsync(Account account)
	{
		_context.Accounts.Update(account);
		await _context.SaveChangesAsync();
	}

	// ACTION: Delete User From the Database
	public async Task DeleteUserAsync(int accountId)
	{
		var account = await _context.Accounts.FindAsync(accountId);
		if (account != null)
		{
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
		}
	}

	#endregion


	#region == Function operation ==

	// SELECT: Get User by AccountID From the Database
	public async Task<Account> GetUserByIdAsync(int accountId)
	{
		return await _context.Accounts.FindAsync(accountId);
	}

	// Get "Username" from the Account table in the Database
	public async Task<Account> GetByUsernameAsync(string username)
	{
		return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
	}

	// Get "Email" from the Account table in the Database
	public async Task<Account> GetByEmailAsync(string email)
	{
		return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
	}

	#endregion
}