using System.Net.Http.Headers;
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

		public async Task<AccountGame> GetAccountGameById(int accountGameId)
		{
			var accountGame = await _accountGameRepository.GetAll();
			return accountGame.SingleOrDefault(ag => ag.AccountGameId == accountGameId);
		}

		public async Task<AccountGame> UpdateAccountGame(AccountGame accountGame)
		{
			var existingAccountGame = await GetAccountGameById(accountGame.AccountGameId);

			if (existingAccountGame == null) throw new Exception("AccountGame does not exist.");

			existingAccountGame.AccountGameId = accountGame.AccountGameId;
			existingAccountGame.AccountId = accountGame.AccountId;
			existingAccountGame.GameId = accountGame.GameId;
			existingAccountGame.DateAdded = accountGame.DateAdded;

			await _accountGameRepository.Update(existingAccountGame);
			return existingAccountGame;
		}

		public async Task DeleteAccountGame(int accountGameId)
		{
			var existingAccountGame = await GetAccountGameById(accountGameId);
			if (existingAccountGame == null) throw new Exception("AccountGame not found!");

			await _accountGameRepository.Delete(existingAccountGame);
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
