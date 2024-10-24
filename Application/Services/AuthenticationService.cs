using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

// Domain
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUserRepository _userRepository;
		private readonly string _secretKey = "Empty"; // TODO: Apply secret key

		public AuthenticationService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		#region == Basic CRUB Function ==
		// SELECT: Get All User
		public async Task<IEnumerable<Account>> GetAllUser()
		{
			return await _userRepository.GetAllUserAsync();
		}

		// ACTION: Update User
		public async Task UpdateUser(Account account)
		{
			await _userRepository.UpdateUserAsync(account);
		}

		// ACTION: Delete User
		public async Task DeleteUser(int accountId)
		{
			var account = await _userRepository.GetUserByIdAsync(accountId);
			if (account != null) // FOUND!
			{
				await _userRepository.DeleteUserAsync(accountId);
			}
		}

		#endregion



		#region == Basic operation ==

		// SELECT: Get specific Account using AccountID
		public async Task<Account> GetUserId(int accountId)
		{
			return await _userRepository.GetUserByIdAsync(accountId);
		}

		// SELECT: Get "Username" value by specific Account
		public async Task<Account> GetAccountByUsername(string username)
		{
			return await _userRepository.GetByUsernameAsync(username);
		}

		// SELECT: Get "Email" value by specific Account
		public async Task<Account> GetAccountByEmail(string email)
		{
			return await _userRepository.GetByEmailAsync(email);
		}

		// ACTION: Validate User Credential
		public async Task<bool> ValidateUserCredentialAsync(string username, string password)
		{
			var account = await _userRepository.GetByUsernameAsync(username);
			return account != null && account.Password == password;
		}

		// ACTION: Generate Token for user
		public async Task<string> GenerateTokenAsync(string username)
		{
			// TODO: Implement logic to generate a JWT or other token
			return await Task.FromResult("generated_token");
		}

		#endregion


		#region == Service Application ==

		// ACTION: User Registration
		public async Task<(bool Success, string Message)> RegisterUser(Account account, string confirmPassword)
		{
			// Check if the username and email already exist
			var existingAccountUserName = await GetAccountByUsername(account.Username);
			var existingAccountEmail = await GetAccountByEmail(account.Email);
			if (existingAccountUserName != null && existingAccountEmail != null)
			{
				return (false, "Username and Email already exist");
			}

			if (existingAccountEmail != null)
			{
				return (false, "Email already exists");
			}

			if (existingAccountUserName != null)
			{
				return (false, "Username already exists");
			}

			// Check if the "Password" and the "Confirm Password" are correct
			if (account.Password != confirmPassword)
			{
				return (false, "Password and Confirm Password are not the same");
			}

			// Create a new account
			account.CreatedOn = DateTime.UtcNow;
			account.IsAdmin = "N"; // TODO: Implement authorization logic later

			// TODO: Hash the password before saving it
			// account.Password = HashPassword(account.Password);

			// Save the account to the database
			await _userRepository.AddUserAsync(account);

			return (true, string.Empty);
		}

		// ACTION: User Login
		public async Task<(bool Success, string Message)> LoginUser(Account account)
		{
			// Check if the username exists
			var existingAccount = await GetAccountByUsername(account.Username);

			if (existingAccount == null)
			{
				return (false, "Username does not exist");
			}

			// Validate password
			if (existingAccount.Password != account.Password)
			{
				return (false, "Invalid password");
			}

			// Generate token
			var token = await GenerateTokenAsync(account.Username);

			// Return success with token
			return (true, token);
		}
		#endregion
	}
}
