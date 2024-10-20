using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Domain
using Domain.Entities;
using Domain.Repository;

// Application
using Application.Interfaces;

namespace Application.Services
{
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
            try
            {
                return await _publisherRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all the publishers", ex);
            }
        }

        public async Task<Publisher> AddPublisherAsync(Publisher publisher)
        {
            try
            {
                await _publisherRepository.Add(publisher);
                return publisher;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add the publisher", ex);
            }
        }

        public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
        {
            try
            {
                await _publisherRepository.Update(publisher);
                return publisher;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update the publisher", ex);
            }
        }

        public async Task<Publisher> DeletePublisherAsync(int id)
        {
            var publisher = await _publisherRepository.GetById(id);
            if (publisher == null)
            {
                throw new Exception("Publisher not found.");
            }
            await _publisherRepository.Delete(id);
            return publisher;
        }

        // == Feature Function ==

        // Search by Publisher ID
        public async Task<Publisher> GetPublisherByIdAsync(int id)
        {
            try
            {
                return await _publisherRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get specific publisher id", ex);
            }
        }
    }
}
