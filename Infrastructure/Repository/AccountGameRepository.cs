using DataAccess.EpicGame;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
	public class AccountGameRepository : IAccountGameRepository
	{
		private readonly EpicGameDbContext _context;

		public AccountGameRepository(EpicGameDbContext context)
		{
			_context = context;
		}


		// Add Game into the Account
		public async Task Add(AccountGame accountGame)
		{
			_context.AccountGames.Add(accountGame);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AccountGame>> GetAll()
		{
			return await _context.AccountGames
				.Include(a => a.Account)
				.Include(g => g.Game)
				.ToListAsync();
		}

		public async Task Update(AccountGame accountGame)
		{
			_context.AccountGames.Update(accountGame);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(AccountGame accountGame)
		{
			_context.AccountGames.Remove(accountGame);
			await _context.SaveChangesAsync();
		}
	}
}
