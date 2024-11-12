using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services
{
	public class AccountGameService : IAccountGameService
	{
		private readonly IAccountGameRepository _accountGameRepository;
		private readonly IGameRepository _gameRepository;
		private readonly IAccountRepository _accountRepository;

		public AccountGameService(IAccountGameRepository accountGameRepository, IGameRepository gameRepository, IAccountRepository accountRepository)
		{
			_accountGameRepository = accountGameRepository;
			_gameRepository = gameRepository;
			_accountRepository = accountRepository;
		}

		public async Task<AccountGame> AddAccountGame(AccountGame accountGame)
		{
			await _accountGameRepository.Add(accountGame);
			return accountGame;
		}

		public async Task<IEnumerable<AccountGame>> GetAllAccountGame()
		{
			return await _accountGameRepository.GetAll();
		}

		public async Task<AccountGame> UpdateAccountGame(AccountGame accountGame)
		{
			// Check if accountGame exists (null check)
			if (accountGame == null)
			{
				throw new ArgumentNullException(nameof(accountGame));
			}
    
			// Update the AccountGame using repository
			await _accountGameRepository.Update(accountGame);
    
			return accountGame;
		}

		public async Task<IEnumerable<AccountGame>> GetAccountGameByGameId(int gameId)
		{
			var accountGameList = await _accountGameRepository.GetAll();
			var filteredGame= accountGameList.Where(f => f.GameId == gameId);
			return filteredGame;
		}

		public async Task<IEnumerable<AccountGame>> GetAccountGameByAccountId(int accountId)
		{
			var accountGameList = await _accountGameRepository.GetAll();
			var filteredAccount = accountGameList.Where(f => f.AccountId == accountId);
			return filteredAccount;
		}

	}
}
