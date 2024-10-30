using Domain.Entities;

namespace Domain.Repository;

public interface IRoleRepository
{
	Task<Role> GetById(int id);
	Task<IEnumerable<Role>> GetAll();
	Task<Role> Add(Role role);
	Task Update(Role role);
	Task Delete(int id);
}