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
        // == Basic CRUD operation ==

        // TODO: Create User
        Task AddUserAsync(Account account);

		// TODO: Read User

		// TODO: Update User

		// TODO: Delete User


		// == Function operation ==

		// Get "Username" value from specific Account
        Task<Account> GetAccountByUserNameAsync(string username);

		// Get "Email" value from specific Account
		Task<Account> GetAccountByEmailAsync(string email);


		// == Service Application ==

		// Validate User Credential
        Task<bool> ValidateUserCredentialAsync(string username, string password);
        
        // Generate Token for User
        Task<string> GenerateTokenAsync(string username);
    }
}
