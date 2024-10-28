using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class RoleRepository : IRoleRepository
{
	private readonly EpicGameDbContext _context;

	public RoleRepository(EpicGameDbContext context)
	{
		_context = context;
	}

	public async Task<Role> GetByIdAsync(int id)
	{
		return await _context.Roles.FindAsync(id);
	}

	public async Task<IEnumerable<Role>> GetAllAsync()
	{
		return await _context.Roles.ToListAsync();
	}

	public async Task<Role> AddAsync(Role role)
	{
		await _context.Roles.AddAsync(role);
		await _context.SaveChangesAsync();
		return role;
	}

	public async Task UpdateAsync(Role role)
	{
		_context.Roles.Update(role);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var role = await _context.Roles.FindAsync(id);
		if (role != null)
		{
			_context.Roles.Remove(role);
			await _context.SaveChangesAsync();
		}
	}
}