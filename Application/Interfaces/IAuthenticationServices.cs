using System.Security.Claims;
using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IAuthenticationServices
{

    // ACTION: Validate Account Credential
    Task<bool> ValidateAccountCredential(string username, string password);

	// ACTION: Register Account
	Task<(bool Success, string Result)> RegisterAccount(Account account, string confirmPassword);

	// ACTION: Login Account
	Task<(bool Success, string Result, int AccountId)> LoginAccount(Account account);

}