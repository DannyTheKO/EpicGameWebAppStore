using System.Security.Claims;
using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IAuthenticationServices
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

    public int GetCurrentLoginAccountId(ClaimsPrincipal User);

	#endregion

	#region == Service Application ==
    // ACTION: Validate Account Credential
    Task<bool> ValidateAccountCredential(string username, string password);

	// ACTION: Register Account
	Task<(bool Success, string Result)> RegisterAccount(Account account, string confirmPassword);

	// ACTION: Login Account
	Task<(bool Success, string Result, int AccountId)> LoginAccount(Account account);

	#endregion
}