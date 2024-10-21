using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Domain
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenreService
    {
        // == Basic CRUD Function ==
        public Task<IEnumerable<Genre>> GetAllGenresAsync();
        public Task<Genre> AddGenreAsync(Genre genre);
        public Task<Genre> UpdateGenreAsync(Genre genre);
        public Task<Genre> DeleteGenreAsync(int id);

        // == Feature Function ==

        // Search by Genre ID
        public Task<Genre> GetGenreByIdAsync(int id);


        // TODO: Search By Name
        
    }
}
