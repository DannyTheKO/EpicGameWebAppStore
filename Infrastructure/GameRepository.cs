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
using EpicGameWebAppStore.Infrastructure.DataAccess;


namespace EpicGameWebAppStore.Infrastructure.Repository;

public class GameRepository : IGameRepository
{
    private readonly EpicGameDBContext _context;

    public GameRepository(EpicGameDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAll()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game> GetById(int id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task Add(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Game game)
    {
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}
