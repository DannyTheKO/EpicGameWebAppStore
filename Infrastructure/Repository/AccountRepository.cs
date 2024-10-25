using DataAccess.EpicGame;
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

	// SELECT: Get User by AccountID From the Database
	public async Task<Account> GetUserByIdAsync(int accountId)
	{
		var account = await _context.Accounts.FindAsync(accountId);
		if (account == null)
		{
			throw new NullReferenceException($"Account with ID {accountId} not found.");
		}
		return account;
	}

	// Get "Username" from the Account table in the Database
	public async Task<Account> GetByUsernameAsync(string username)
	{
		var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
		if (account == null)
		{
			throw new NullReferenceException($"Account with Username {username} not found.");
		}
		return account;
	}

	// Get "Email" from the Account table in the Database
	public async Task<Account> GetByEmailAsync(string email)
	{
		var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
		if (account == null)
		{
			throw new NullReferenceException($"Account with Email {email} not found.");
		}
		return account;
	}

	#endregion
}