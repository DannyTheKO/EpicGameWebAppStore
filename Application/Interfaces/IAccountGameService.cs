using Domain.Entities;

namespace Application.Interfaces
{
	public interface IAccountGameService
	{
		// Basic CRUD operations
		Task<AccountGame> AddAccountGame(AccountGame accountGame);
		Task<AccountGame> UpdateAccountGame(AccountGame accountGame);
		Task<IEnumerable<AccountGame>> GetAllAccountGame();

		// Feature operations
		Task<IEnumerable<AccountGame>> GetAccountGameByAccountId(int accountId);
		Task<IEnumerable<AccountGame>> GetAccountGameByGameId(int gameId);
	}
}