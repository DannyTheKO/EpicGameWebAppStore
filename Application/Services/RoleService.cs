using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository, IAccountRepository accountRepository)
    {
        _roleRepository = roleRepository;
        _accountRepository = accountRepository;
    }

    #region == Basic Funciton ==

    // SELECT: Get all Role
    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        return await _roleRepository.GetAll();
    }

    // SELECT: Get Role by Account ID
    public async Task<string> GetRoleById(int accountId)
    {
        var account = await _accountRepository.GetId(accountId);
        if (account == null || account.RoleId == null) return "Guest";

        var role = await _roleRepository.GetById(account.RoleId.Value);
        return role?.Name ?? "Guest";
    }

    #endregion
}