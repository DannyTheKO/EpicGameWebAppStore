using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
	public class AuthorizationServices : IAuthorizationServices
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IRoleRepository _roleRepository;

		public AuthorizationServices(IAccountRepository accountRepository, IRoleRepository roleRepository)
		{
			_accountRepository = accountRepository;
			_roleRepository = roleRepository;
		}

		// TODO: Assign Role to User Function
		public async Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId)
		{
			// Check if that account exist
			var account = await _accountRepository.GetUserByIdAsync(accountId);
			if (account == null) return (false, "Account does not exist"); // If not, we return false

			// Check if that role exist
			var role = await _roleRepository.GetById(roleId);
			if (role == null) return (false, "Role does not exist");  // Role not exist

			// If all passed
			account.RoleId = roleId;
			await _accountRepository.UpdateUserAsync(account);
			return (true, "Role assigned successfully");
		}

		public async Task<IEnumerable<Role>> GetAllRoles()
		{
			return await _roleRepository.GetAllRoleAsync();
		}
	}
}
