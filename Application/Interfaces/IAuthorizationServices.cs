using System.Security.Claims;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthorizationServices
{
    // ACTION: Assign role to user
	Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId);


	// VALIDATE: Check if that user has that specific permission
	Task<bool> UserHasPermission(int accountId, string requiredPermission);

	// ACTION: Claim Identity User
	Task<ClaimsPrincipal> CreateClaimsPrincipal(int accountId);
}