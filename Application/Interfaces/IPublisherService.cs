using Domain.Entities;
// Domain

namespace Application.Interfaces;

public interface IPublisherService
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Publisher>> GetAllPublishers();
    public Task<Publisher> AddPublisher(Publisher publisher);
    public Task<Publisher> UpdatePublisher(Publisher publisher);
    public Task<Publisher> DeletePublisher(int id);

    // == Feature Function ==

    // Search by Publisher ID
    public Task<Publisher> GetPublisherById(int id);

    public Task<IEnumerable<Publisher>> GetPublisherByName(string name);

    public Task<IEnumerable<Publisher>> GetPublisherByAddress(string address);
}