﻿using System.Text.Json.Nodes;
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

    // SELECT: Get all Role
    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        return await _roleRepository.GetAll();
    }

    // SELECT: Get Role detail by RoleId
    public async Task<Role> GetRoleById(int roleId)
    {
	    return await _roleRepository.GetById(roleId);
    }

    // SELECT: Get Role by Account ID
    public async Task<Role> GetRoleByAccountId(int accountId)
    {
        var account = await _accountRepository.GetId(accountId);
		if (account == null || account.RoleId == null) throw new Exception("Account not found or RoleId is null");

        var role = await _roleRepository.GetById(account.RoleId);
        return role;
    }

    // SELECT: Get Role by Name
    public async Task<IEnumerable<Role>> GetRoleByName(string roleName)
    {
	    var roleList = await _roleRepository.GetAll();
	    var filteredRole = roleList.Where(f => f.Name == roleName);
        return filteredRole;
	}
}