using System.Security.Claims;
using Application.Interfaces;
using Domain.Repository;

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


    // ACTION: Assign Role to User Function
    public async Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId)
    {
        // Check if that account exist
        var account = await _accountRepository.GetId(accountId);
        if (account == null) return (false, "Account does not exist"); // If not, we return false

        // Check if that role exist
        var role = await _roleRepository.GetById(roleId);
        if (role == null) return (false, "Role does not exist"); // Role not exist

        // If all passed
        account.RoleId = roleId;
        await _accountRepository.Update(account);
        return (true, "Role assigned successfully");
    }

    // VALIDATE: Check that user has permission
    public async Task<bool> UserHasPermission(int accountId, string requiredPermission)
    {
	    var account = await _accountRepository.GetId(accountId);
	    if (account == null) return false;

	    // Get role directly using the RoleId from account
	    var role = await _roleRepository.GetById(account.RoleId);
	    if (role == null) return false;

	    return role.Permission != null && role.Permission.Contains(requiredPermission);
    }
}