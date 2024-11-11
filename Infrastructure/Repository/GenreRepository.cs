using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
// Domain

// Infrastructure

namespace Infrastructure.Repository;

public class GenreRepository : IGenreRepository
{
    private readonly EpicGameDbContext _context;

    public GenreRepository(EpicGameDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre> GetById(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task Add(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Genre genre)
    {
        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}

