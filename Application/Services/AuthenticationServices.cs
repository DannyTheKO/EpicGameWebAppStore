using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;

    public AuthenticationServices(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
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
    public async Task<(bool LoginState, string Token, string ResultMessage)> LoginAccount(Account account)
    {
	    var existingAccount = await _accountService.GetAccountByUsername(account.Username);
	    if (existingAccount == null) return (false, null, "Username does not exist");
	    if (existingAccount.Password != account.Password) return (false, null, "Invalid password");
    
	    // Generate JWT token instead of cookie authentication
	    var token = GenerateJwtToken(existingAccount);
	    return (true, token, "Successfully login");
    }

    public string GenerateJwtToken(Account account)
    {
	    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
	    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

	    var claims = new[]
	    {
		    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
		    new Claim(ClaimTypes.Name, account.Username),
		    new Claim(ClaimTypes.Role, account.RoleId.ToString()),
		    new Claim(ClaimTypes.Email, account.Email),
	    };

	    var token = new JwtSecurityToken(
		    issuer: _configuration["Jwt:Issuer"],
		    audience: _configuration["Jwt:Audience"],
		    claims: claims,
		    expires: DateTime.UtcNow.AddMinutes(30),
		    signingCredentials: credentials
	    );

	    return new JwtSecurityTokenHandler().WriteToken(token);
    }
}