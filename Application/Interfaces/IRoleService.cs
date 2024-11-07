using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        // SELECT: Get all available roles
        Task<IEnumerable<Role>> GetAllRoles();

        // SELECT: Get Role by Account ID
        Task<string> GetRoleById(int accountId);
    }
}
