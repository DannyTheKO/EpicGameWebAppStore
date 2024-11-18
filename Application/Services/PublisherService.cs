using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
// Domain

// Application

namespace Application.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

	// == Basic CRUD Function ==
	public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
	{
		return await _publisherRepository.GetAll();
	}

	public async Task<Publisher> AddPublisherAsync(Publisher publisher)
	{
		await _publisherRepository.Add(publisher);
		return publisher;
	}

	public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
	{
		await _publisherRepository.Update(publisher);
		return publisher;
	}

    public async Task<Publisher> DeletePublisherAsync(int id)
    {
        var publisher = await _publisherRepository.GetById(id);
        if (publisher == null) throw new Exception("Publisher not found.");
        await _publisherRepository.Delete(id);
        return publisher;
    }

	// == Feature Function ==

	// Search by Publisher ID
	public async Task<Publisher> GetPublisherByIdAsync(int id)
	{
		return await _publisherRepository.GetById(id);
	}

	public async Task<IEnumerable<Publisher>> GetPublisherByNameAsync(string name)
	{
		var publisherList = await _publisherRepository.GetAll();
		var filteredPublisher = publisherList.Where(f => f.Name == name);
		return filteredPublisher;
	}

	public async Task<IEnumerable<Publisher>> GetPublisherByAddressAsync(string address)
	{
		var publisherList = await _publisherRepository.GetAll();
		var filteredPublisher = publisherList.Where(f => f.Address == address);
		return filteredPublisher;
	}
}