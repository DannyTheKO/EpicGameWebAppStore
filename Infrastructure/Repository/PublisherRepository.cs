using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Domain
using Domain.Entities;
using Domain.Repository;

// Infrastructure
using Infrastructure.DataAccess;

namespace Infrastructure.Repository
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly EpicGameDbContext _context;

        public PublisherRepository(EpicGameDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAll()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher> GetById(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }

        public async Task Add(Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }
    }
}
