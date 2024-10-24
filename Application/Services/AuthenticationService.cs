using System;
using System.Collections.Generic;
using System.Linq;
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

		#region == Basic CRUD operation ==

		// ACTION: Create User
		public async Task AddUser(Account account)
		{
			await _userRepository.AddUserAsync(account);
		}

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



		#region == Function operation ==

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

		#endregion


		#region == Service Application ==
		public async Task<bool> ValidateUserCredentialAsync(string username, string password)
		{
			var account = await _userRepository.GetByUsernameAsync(username);
			return account != null && account.Password == password;
		}

		public async Task<string> GenerateTokenAsync(string username)
		{
			// TODO: Implement logic to generate a JWT or other token
			return await Task.FromResult("generated_token");
		}

		#endregion
	}
}
