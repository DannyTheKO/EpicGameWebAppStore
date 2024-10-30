using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using System.Security.Claims;

namespace Application.Services;

public class AuthorizationServices : IAuthorizationServices
{
	private readonly IAccountRepository _accountRepository;
	private readonly IRoleRepository _roleRepository;

	public AuthorizationServices(IAccountRepository accountRepository, IRoleRepository roleRepository)
	{
		_accountRepository = accountRepository;
		_roleRepository = roleRepository;
	}

	#region == Basic Funciton ==
	// SELECT: This function will be somewhere else
	public async Task<IEnumerable<Role>> GetAllRoles()
	{
		return await _roleRepository.GetAllAsync();
	}

	public async Task<String> GetUserRole(int accountId)
	{
		var account = await _accountRepository.GetUserByIdAsync(accountId);
		if (account == null || account.RoleId == null) return "Guest";

		var role = await _roleRepository.GetByIdAsync(account.RoleId.Value);
		return role?.Name ?? "Guest";
	}
	
	#endregion

	#region == Service Function ==
	// ACTION: Assign Role to User Function
	public async Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId)
	{
		// Check if that account exist
		var account = await _accountRepository.GetUserByIdAsync(accountId);
		if (account == null) return (false, "Account does not exist"); // If not, we return false

		// Check if that role exist
		var role = await _roleRepository.GetByIdAsync(roleId);
		if (role == null) return (false, "Role does not exist"); // Role not exist

		// If all passed
		account.RoleId = roleId;
		await _accountRepository.UpdateUserAsync(account);
		return (true, "Role assigned successfully");
	}

	// VALIDATE: Check that user has permission
	public async Task<bool> UserHasPermission(int accountId, string permission)
	{
		var account = await _accountRepository.GetUserByIdAsync(accountId);
		if (account == null || account.Role == null) return false;

		var role = await _roleRepository.GetByIdAsync(account.Role.RoleId);
		return role.Permission != null && role.Permission.Contains(permission);
	}


	// Create claim principal when user is login
	public async Task<ClaimsPrincipal> CreateClaimsPrincipal(int accountId)
	{
		var accountUsername = await _accountRepository.GetUserByIdAsync(accountId);
		var accountRole = await _accountRepository.GetUserRoleAsync(accountId);
		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, accountUsername.Username),
			new(ClaimTypes.Role, accountRole),
		};

		var identity = new ClaimsIdentity(claims, "CookieAuth");
		return await Task.FromResult(new ClaimsPrincipal(identity));
	}
	
	#endregion

}