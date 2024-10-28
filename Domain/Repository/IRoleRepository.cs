using Domain.Entities;

namespace Domain.Repository;

public interface IRoleRepository
{
	Task<Role> GetByIdAsync(int id);
	Task<IEnumerable<Role>> GetAllAsync();
	Task<Role> AddAsync(Role role);
	Task UpdateAsync(Role role);
	Task DeleteAsync(int id);
}