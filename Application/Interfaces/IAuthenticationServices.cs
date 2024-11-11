using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IAuthenticationServices
{
    // ACTION: Validate Account Credential
    Task<bool> ValidateAccountCredential(string username, string password);

    // ACTION: Register Account
    Task<(bool RegisterStage, string ResultMessage)> RegisterAccount(Account account, string confirmPassword);

    // ACTION: Login Account
    Task<(bool LoginState, string ResultMessage, int AccountId)> LoginAccount(Account account);
}