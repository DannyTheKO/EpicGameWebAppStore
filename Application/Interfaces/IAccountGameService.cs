using Domain.Entities;

namespace Application.Interfaces
{
	public interface IAccountGameService
	{
		// Basic CRUD operations
		Task<AccountGame> AddAccountGame(AccountGame accountGame);
		Task<IEnumerable<AccountGame>> GetAllAccountGame();
		Task<AccountGame> GetAccountGameById(int accountGameId);
		Task<AccountGame> UpdateAccountGame(AccountGame accountGame);
		Task DeleteAccountGame(int accountGameId);

		// Feature operations
		Task<IEnumerable<AccountGame>> GetAccountGameByGameId(int gameId);
		Task<IEnumerable<AccountGame>> GetAccountGameByAccountId(int accountId);
	}
}