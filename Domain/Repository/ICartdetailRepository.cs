using Domain.Entities;

namespace Domain.Repository;

public interface ICartdetailRepository
{
    Task<IEnumerable<Cartdetail>> GetAll();
    Task<Cartdetail> GetById(int id);
    Task Add(Cartdetail cartdetail);
    Task Create(Cartdetail cartdetail);
    Task Update(Cartdetail cartdetail);
    Task Delete(int id);
    Task<IEnumerable<Cartdetail>> GetByCartId(int cartId);
}