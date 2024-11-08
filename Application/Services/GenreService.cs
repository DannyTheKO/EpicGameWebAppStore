using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
// Domain

// Application

namespace Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    // == Basic CRUD Function ==
    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        try
        {
            return await _genreRepository.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get all the genres", ex);
        }
    }

    public async Task<Genre> AddGenreAsync(Genre genre)
    {
        try
        {
            await _genreRepository.Add(genre);
            return genre;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add the genre", ex);
        }
    }

    public async Task<Genre> UpdateGenreAsync(Genre genre)
    {
        try
        {
            await _genreRepository.Update(genre);
            return genre;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update the genre", ex);
        }
    }

    public async Task<Genre> DeleteGenreAsync(int id)
    {
        var genre = await _genreRepository.GetById(id);
        if (genre == null) throw new Exception("Genre not found.");
        await _genreRepository.Delete(id);
        return genre;
    }

    // == Feature Function ==

    // Search by Genre ID
    public async Task<Genre> GetGenreByIdAsync(int id)
    {
        try
        {
            return await _genreRepository.GetById(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get specific genre id", ex);
        }
    }
}