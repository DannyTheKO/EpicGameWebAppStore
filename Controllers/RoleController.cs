using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class RoleController : _BaseController
{
    private readonly IRoleService _roleService;
    private readonly IAuthorizationServices _authorizationServices;

	public RoleController(IRoleService roleService, IAuthorizationServices authorizationServices) : base(authorizationServices)
	{
        _roleService = roleService;
    }

    // Get all roles
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRoles();   
        return Ok(roles);
    }

    // Get role by ID
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRoleById(int roleId)
    {
        var role = await _roleService.GetRoleById(roleId);
        if (role == null)
        {
            return NotFound("Role not found.");
        }
        return Ok(role);
    }

    // Get role by account ID
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetRoleByAccountId(int accountId)
    {
        try
        {
            var role = await _roleService.GetRoleByAccountId(accountId);
            return Ok(role);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Get roles by name
    [HttpGet("name/{roleName}")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var roles = await _roleService.GetRoleByName(roleName);
        return Ok(roles);
    }
}
