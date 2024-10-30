using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
	private readonly IAccountRepository _accountRepository;
	private readonly IRoleRepository _roleRepository;
	private readonly string _secretKey = "Empty"; // TODO: Apply secret key

	public AuthenticationServices(IAccountRepository accountRepository, IRoleRepository roleRepository)
	{
		_accountRepository = accountRepository;
		_roleRepository = roleRepository;
	}

	#region == Basic CRUB Function ==

	// SELECT: Get All User
	public async Task<IEnumerable<Account>> GetAllUser()
	{
		return await _accountRepository.GetAllUserAsync();
	}

	// ACTION: Update User
	public async Task<Account> UpdateUser(Account account)
	{
		if (account == null) // NOT FOUND!
			throw new ArgumentNullException(nameof(account), "Account cannot be null");

		// Retrieve the existing account from the database
		var existingAccount = await _accountRepository.GetUserByIdAsync(account.AccountId);
		if (existingAccount == null) throw new Exception("Account not found");

		// Update the account details
		existingAccount.Username = account.Username;
		existingAccount.Email = account.Email;
		existingAccount.Password = account.Password; // Consider hashing the password
		existingAccount.IsActive = account.IsActive;

		// Save the updated account to the database
		await _accountRepository.UpdateUserAsync(existingAccount);

		return existingAccount;
	}

	// ACTION: Delete User
	public async Task DeleteUser(int accountId)
	{
		var account = await _accountRepository.GetUserByIdAsync(accountId);
		if (account != null) // FOUND!
			await _accountRepository.DeleteUserAsync(accountId);
	}

	#endregion

	#region == Basic operation ==

	// SELECT: Get specific Account using AccountID
	public async Task<Account> GetUserId(int accountId)
	{
		return await _accountRepository.GetUserByIdAsync(accountId);
	}

	// SELECT: Get "Username" value by specific Account
	public async Task<Account> GetAccountByUsername(string username)
	{
		return await _accountRepository.GetByUsernameAsync(username);
	}

	// SELECT: Get "Email" value by specific Account
	public async Task<Account> GetAccountByEmail(string email)
	{
		return await _accountRepository.GetByEmailAsync(email);
	}

	// ACTION: Validate User Credential
	public async Task<bool> ValidateUserCredentialAsync(string username, string password)
	{
		var account = await _accountRepository.GetByUsernameAsync(username);
		return account != null && account.Password == password;
	}

	// ACTION: Generate Token for user
	public async Task<string> GenerateTokenAsync(string username)
	{
		// TODO: Implement logic to generate a JWT or other token
		return await Task.FromResult("generated_token");
	}

	#endregion

	#region == Service Application ==

	// ACTION: User Registration
	public async Task<(bool Success, string Result)> RegisterUser(Account account, string confirmPassword)
	{
		// Check if the username and email already exist
		var existingAccountUserName = await GetAccountByUsername(account.Username);
		var existingAccountEmail = await GetAccountByEmail(account.Email);
		if (existingAccountUserName != null && existingAccountEmail != null)
			return (false, "Username and Email already exist");

		if (existingAccountEmail != null) return (false, "Email already exists");

		if (existingAccountUserName != null) return (false, "Username already exists");

		// Check if the "Password" and the "Confirm Password" are correct
		if (account.Password != confirmPassword) return (false, "Password and Confirm Password are not the same");

		// Create a new account
		account.CreatedOn = DateTime.UtcNow;
		account.RoleId = 5; // Default Role is "Guest"
		account.IsActive = "Y"; // TODO: Implement authorization logic later

		// TODO: Hash the password before saving it
		// account.Password = HashPassword(account.Password);

		// Save the account to the database
		await _accountRepository.AddUserAsync(account);

		return (true, string.Empty);
	}

	// ACTION: User Login
	public async Task<(bool Success, string Result, int AccountId)> LoginUser(Account account)
	{
		// Check if the username exists
		var existingAccount = await GetAccountByUsername(account.Username);

		if (existingAccount == null) return (false, "Username does not exist", 0);

		// Validate password
		if (existingAccount.Password != account.Password) return (false, "Invalid password", 0);

		// Generate token
		var result = await GenerateTokenAsync(account.Username);

		// Return success with token and AccountId
		return (true, result, existingAccount.AccountId);
	}


	#endregion
}