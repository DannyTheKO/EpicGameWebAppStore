using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
	public class RoleRepository : IRoleRepository
	{
		private readonly EpicGameDbContext _context;

		public RoleRepository(EpicGameDbContext context)
		{
			_context = context;
		}

		// ACTION: Create Role from the Database
		public async Task AddRoleAsync(Role role)
		{
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();
		}

		// SELECT: Get ALL Role from the Database
		public async Task<IEnumerable<Role>> GetAllRoleAsync()
		{
			return await _context.Roles.ToListAsync();
		}

		// SELECT: Get single Role using ID from the Database
		public async Task<Role> GetById(int roleId)
		{
			var role = await _context.Roles.FindAsync(roleId);
			if (role != null) // FOUND!
			{
				return role;
			}
			else
			{
				// if RoleId is not found!
				throw new NullReferenceException($"Role with ID {roleId} not found.");
			}
		}

		// ACTION: Update Role from the Database
		public async Task UpdateRoleAsync(Role role)
		{
			_context.Roles.Update(role);
			await _context.SaveChangesAsync();
		}

		// ACTION: Delete Role from the Database
		public async Task DeleteRoleAsync(int roleId)
		{
			var role = await _context.Roles.FindAsync(roleId);
			if (role != null) // FOUND!
			{
				_context.Roles.Remove(role);
				await _context.SaveChangesAsync();
			}
		}
	}
}
