using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
	public interface IRoleRepository
	{
		// ACTION: Create Role from the Database
		Task AddRoleAsync(Role role);
		
		// SELECT: Get ALL Role from the Database
		Task<IEnumerable<Role>> GetAllRoleAsync();

		// SELECT: Get single Role using ID from the Database
		Task<Role> GetById(int id);


		// ACTION: Update Role from the Database
		Task UpdateRoleAsync(Role role);

		// ACTION: Delete Role from the Database
		Task DeleteRoleAsync(int roleId);
	}
}
