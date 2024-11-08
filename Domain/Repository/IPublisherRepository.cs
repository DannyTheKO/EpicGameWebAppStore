using Domain.Entities;
// Domain

namespace Domain.Repository;

public interface IPublisherRepository
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Publisher>> GetAll();
    public Task Add(Publisher publisher);
    public Task Update(Publisher publisher);
    public Task Delete(int id);

    // == Feature Function ==

    // Search by Publisher ID
    public Task<Publisher> GetById(int id);

    // TODO: Search By Name
    // TODO: Search By Address
}