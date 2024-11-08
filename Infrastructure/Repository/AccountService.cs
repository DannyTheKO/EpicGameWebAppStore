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

    // ACTION: Create User
    public async Task Add(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    // SELECT: Get all User 
    public async Task<IEnumerable<Account>> GetAll()
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .ToListAsync();
    }

    // ACTION: Update User
    public async Task Update(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    // ACTION: Delete User
    public async Task Delete(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region == Select Operation ==

    // SELECT: Get Account by AccountID
    public async Task<Account> GetId(int accountId)
    {
        return await _context.Accounts.FindAsync(accountId);
    }

    // Get "Username" from the Account table in the Database
    public async Task<Account> GetUsername(string username)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
    }

    // Get "Email" from the Account table in the Database
    public async Task<Account> GetEmail(string email)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<string> GetUserRoleAsync(int accountId)
    {
        var account = await GetId(accountId);
        if (account == null || account.RoleId == null) return "Guest"; // Default role

        var role = await _context.Roles.FindAsync(account.RoleId);
        return role?.Name ?? "Guest";
    }

    #endregion
}