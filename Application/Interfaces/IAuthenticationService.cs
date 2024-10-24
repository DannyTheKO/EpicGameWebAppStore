using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using Domain.Entities;

namespace Application.Interfaces
{
	public interface IAuthenticationService
	{
		#region == Basic CRUB Function ==
		// SELECT: Get all user
		Task<IEnumerable<Account>> GetAllUser();

		// ACTION: Delete User
		Task DeleteUser(int AccountId);
		
		#endregion

		
		#region == Basic operation ==
		// SELECT: Get Username by ID Account
		Task<Account> GetUserId(int accountId);

		// SELECT: Get "Username" value from specific Account
		Task<Account> GetAccountByUsername(string username);

		// SELECT: Get "Email" value from specific Account
		Task<Account> GetAccountByEmail(string email);

		// ACTION: Validate User Credential
		Task<bool> ValidateUserCredentialAsync(string username, string password);

		// ACTION: Generate Token for User
		Task<string> GenerateTokenAsync(string username);
		
		#endregion

		#region == Service Application ==
		// ACTION: Register User
		Task<(bool Success, string Message)> RegisterUser(Account account, string confirmPassword);

		// ACTION: Login User
		Task<(bool Success, string Message)> LoginUser(Account account);

		// ACTION: Update User
		Task<Account> UpdateUser(Account account);

		#endregion

	}
}
