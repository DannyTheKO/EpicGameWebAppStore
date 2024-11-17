using Domain.Entities;
// Domain

namespace Application.Interfaces;

public interface IGenreService
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Genre>> GetAllGenres();
    public Task<Genre> AddGenre(Genre genre);
    public Task<Genre> UpdateGenre(Genre genre);
    public Task<Genre> DeleteGenre(int id);

    // == Feature Function ==

    // Search by Genre ID
    public Task<Genre> GetGenreById(int id);

    // TODO: Search By Name
    public Task<IEnumerable<Genre>> GetGenreByName(string name);


}