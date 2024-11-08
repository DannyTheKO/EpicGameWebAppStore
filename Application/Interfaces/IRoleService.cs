using Domain.Entities;

namespace Application.Interfaces;

public interface IRoleService
{
    // SELECT: Get all available roles
    Task<IEnumerable<Role>> GetAllRoles();

    // SELECT: Get Role by Account ID
    Task<string> GetRoleById(int accountId);
}