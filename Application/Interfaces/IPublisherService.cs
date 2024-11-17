using Domain.Entities;
// Domain

namespace Application.Interfaces;

public interface IPublisherService
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Publisher>> GetAllPublishersAsync();
    public Task<Publisher> AddPublisherAsync(Publisher publisher);
    public Task<Publisher> UpdatePublisherAsync(Publisher publisher);
    public Task<Publisher> DeletePublisherAsync(int id);

    // == Feature Function ==

    // Search by Publisher ID
    public Task<Publisher> GetPublisherByIdAsync(int id);

    // TODO: Search By Name
    public Task<IEnumerable<Publisher>> GetPublisherByNameAsync(string name);

	// TODO: Search By Address
    public Task<IEnumerable<Publisher>> GetPublisherByAddressAsync(string address);
}