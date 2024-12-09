using System.Security.Claims;
using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IAuthenticationServices
{
	// ACTION: Get current login Account
	public int GetLoginAccountId(ClaimsPrincipal user);

	// ACTION: Validate Account Credential
	Task<bool> ValidateAccountCredential(string username, string password);

    // ACTION: Register Account
    Task<(bool RegisterStage, string ResultMessage)> RegisterAccount(Account account, string confirmPassword);

    // ACTION: Login Account
    Task<(bool LoginState, string Token, string ResultMessage)> LoginAccount(Account account);

	// ACTION: Forgot Password
	Task<(bool ForgotPasswordState, string ResultMessage)> ForgotPassword(string username, string email);

	// ACTION: Generate JWT Token
	Task<string> GenerateJwtToken(Account account);
} 