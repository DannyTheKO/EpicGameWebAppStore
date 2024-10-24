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

        // ACTION: Create User
        Task AddUser(Account account);

		// SELECT: Get all user
		Task<IEnumerable<Account>> GetAllUser();

		// ACTION: Update User
		Task UpdateUser(Account account);

		// ACTION: Delete User
		Task DeleteUser(int AccountId);


		// == Function operation ==

        // SELECT: Get Username by ID Account
        Task<Account> GetUserId(int accountId);

		// SELECT: Get "Username" value from specific Account
        Task<Account> GetAccountByUsername(string username);

		// SELECT: Get "Email" value from specific Account
		Task<Account> GetAccountByEmail(string email);


		// == Service Application ==

		// ACTION: Validate User Credential
        Task<bool> ValidateUserCredentialAsync(string username, string password);
        
        // ACTION: Generate Token for User
        Task<string> GenerateTokenAsync(string username);
    }
}
