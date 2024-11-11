using System.Security.Claims;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAccountService
{
    #region == Basic CRUB Function ==

    //ACTION: Create Account
    Task<Account> AddAccount(Account account);

    // SELECT: Get all Account
    Task<IEnumerable<Account>> GetAllAccounts();

    // ACTION: Update Account
    Task<Account> UpdateAccount(Account account);

    // ACTION: Delete Account
    Task DeleteAccount(int accountId);

    #endregion

    #region == Basic operation ==

    // SELECT: Get Username by ID Account
    Task<Account> GetAccountById(int accountId);

    // SELECT: Get "Username" value from specific Account
    Task<Account> GetAccountByUsername(string username);

    // SELECT: Get "Email" value from specific Account
    Task<Account> GetAccountByEmail(string email);

    public int GetLoginAccountId(ClaimsPrincipal User);

    #endregion
}