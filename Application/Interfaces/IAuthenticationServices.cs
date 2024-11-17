using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IAuthenticationServices
{
	// ACTION: Get current login Account
	public int GetLoginAccountId(ClaimsPrincipal User);

	// ACTION: Validate Account Credential
	Task<bool> ValidateAccountCredential(string username, string password);

    // ACTION: Register Account
    Task<(bool RegisterStage, string ResultMessage)> RegisterAccount(Account account, string confirmPassword);

    // ACTION: Login Account
    Task<(bool LoginState, string Token, string ResultMessage)> LoginAccount(Account account);

	// ACTION: Generate JWT Token
	public string GenerateJwtToken(Account account);
}