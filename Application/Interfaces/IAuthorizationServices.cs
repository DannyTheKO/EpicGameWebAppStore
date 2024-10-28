using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthorizationServices
{
	Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId);
	Task<IEnumerable<Role>> GetAllRoles();
	Task<bool> UserHasPermission(int accountId, string permission);
}