using DataAccess;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
// Domain

// Infrastructure


namespace Infrastructure.Repository;

public class GameRepository : IGameRepository
{
	private readonly EpicGameDbContext _context;

	public GameRepository(EpicGameDbContext context)
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