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
	private readonly IRoleService _roleService;
	private readonly IConfiguration _configuration;

	public AuthenticationServices(IAccountService accountService, IRoleService roleService, IConfiguration configuration)
	{
		_accountService = accountService;
		_roleService = roleService;
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
		account.RoleId = 3;
		account.IsActive = "Y";

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

		// Check is that account is allow to be active
		if (existingAccount?.IsActive == "N") return (false, null, "Account not allow to be active.");

		// Check Username exist in Account database
		if (existingAccount == null) return (false, null, "Invalid Username.");

		// If Username exists, check if the password is correct
		if (existingAccount.Password != account.Password) return (false, null, "Invalid Password.");

		// Generate JWT token instead of cookie authentication
		var token = GenerateJwtToken(existingAccount);
		return (true, token.Result, "Successfully login");
	}

	// ACTION: Forgot Password
	public async Task<(bool ForgotPasswordState, string ResultMessage)> ForgotPassword(string username, string email)
	{
		// Check if username exists
		var accountByUsername = await _accountService.GetAccountByUsername(username);
		if (accountByUsername == null)
		{
			return (false, "Username not found");
		}

		// Check if email exists
		var accountByEmail = await _accountService.GetAccountByEmail(email);
		if (accountByEmail == null)
		{
			return (false, "Email address not found");
		}

		// Verify that username and email belong to the same account
		if (accountByUsername.AccountId != accountByEmail.AccountId)
		{
			return (false, "Username and email do not match");
		}

		// Check if account is active
		if (accountByUsername.IsActive == "N")
		{
			return (false, "Account is not active");
		}

		// Generate temporary password (8 characters)
		var tempPassword = Guid.NewGuid().ToString("N").Substring(0, 8);

		// Update account with temporary password
		accountByUsername.Password = tempPassword;
		await _accountService.UpdateAccount(accountByUsername);

		// In a production environment, you would send an email here
		return (true, $"Temporary password has been generated and sent to your email: {email} / {tempPassword}");
	}


	public async Task<string> GenerateJwtToken(Account account)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var role = await _roleService.GetRoleByAccountId(account.AccountId);
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
			new Claim(ClaimTypes.Name, account.Username),
			new Claim(ClaimTypes.Role, role.Name),  // This is what the [Authorize] attribute checks
		    new Claim(ClaimTypes.Email, account.Email),
			new Claim("IsActive", account.IsActive),
			new Claim("Permission", role.Permission.ToString())
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

	public int GetLoginAccountId(ClaimsPrincipal user)
	{
		return int.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
	}
}