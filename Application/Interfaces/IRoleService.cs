using Domain.Entities;

namespace Application.Interfaces;

public interface IRoleService
{
    // SELECT: Get all available roles
    Task<IEnumerable<Role>> GetAllRoles();

    // SELECT: Get Role detail by RoleId
    Task<Role> GetRoleById(int roleId);

    // SELECT: Get Role by Account ID
    Task<Role> GetRoleByAccountId(int accountId);

    Task<IEnumerable<Role>> GetRoleByName(string roleName);
}