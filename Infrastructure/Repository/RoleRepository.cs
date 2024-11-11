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

    #region == Select Operation ==

    // SELECT: Get Role by ID
    public async Task<Role> GetById(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    #endregion

    #region == Basic CRUB ==

    // ACTION: Add Role
    public async Task<Role> Add(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    // SELECT: Get all Role
    public async Task<IEnumerable<Role>> GetAll()
    {
        return await _context.Roles.ToListAsync();
    }

    // ACTION: Update Role
    public async Task Update(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    // ACTION: Delete Role
    public async Task Delete(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    #endregion
}