using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Application.Interfaces
{
	public interface IAuthorizationServices
	{
		Task<(bool Success, string Message)> AssignRoleToUser(int accountId, int roleId);
		Task<IEnumerable<Role>> GetAllRoles();
		// Add other methods as needed

	}
}
