using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Domain
using Domain.Entities;

namespace Application.Interfaces
{
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
        // TODO: Search By Address

    }
}
