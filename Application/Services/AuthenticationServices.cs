using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IAccountService _accountService;

    public AuthenticationServices(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // ACTION: Validate User Credential
    public async Task<bool> ValidateAccountCredential(string username, string password)
    {
        var account = await _accountService.GetAccountByUsername(username);
        return account != null && account.Password == password;
    }

    // ACTION: User Registration
    public async Task<(bool RegisterStage, string ResultMessage)> RegisterAccount(Account account, string confirmPassword)
    {
        // Check if the username and email already exist
        var existingAccountUserName = await _accountService.GetAccountByUsername(account.Username);
        var existingAccountEmail = await _accountService.GetAccountByEmail(account.Email);
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
        await _accountService.AddAccount(account);

        return (true, string.Empty);
    }

    // ACTION: User Login
    public async Task<(bool LoginState, string ResultMessage, int AccountId)> LoginAccount(Account account)
    {
        // Check if the username exists
        var existingAccount = await _accountService.GetAccountByUsername(account.Username);

        if (existingAccount == null) return (false, "Username does not exist", 0);

        // Validate password
        if (existingAccount.Password != account.Password) return (false, "Invalid password", 0);

        // Generate token
        var result = "Successfully login";

        // Return success with token and AccountId
        return (true, result, existingAccount.AccountId);
    }
}