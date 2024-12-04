using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;


[Route("[controller]")]
[ApiController]
[Authorize()]
public class AccountGameController : _BaseController
{
	private readonly IAccountGameService _accountGameService;
	private readonly IAccountService _accountService;
	private readonly IGameService _gameService;
	private readonly IAuthorizationServices _authorizationServices;

	public AccountGameController(IAccountGameService accountGameService, IAccountService accountService, IGameService gameService, IAuthorizationServices authorizationServices) 
		: base (authorizationServices)
	{
		_accountGameService = accountGameService;
		_accountService = accountService;
		_gameService = gameService;
	}

	[HttpGet("GetAllAccountGame")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAllAccountGame()
	{
		var accountGameList = await _accountGameService.GetAllAccountGame();
		return Ok(accountGameList);
	}

	[HttpGet("GetAccountGameById/{accountGameId}")]
	public async Task<ActionResult<AccountGame>> GetAccountGameById(int accountGameId)
	{
		var accountGame = await _accountGameService.GetAccountGameById(accountGameId);
		return Ok(accountGame);
	}

	[HttpGet("GetAccountGameByGameId/{gameId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByGameId(int gameId)
	{
		// Retrieve Game by Game ID
		var accountGameList = await _accountGameService.GetAccountGameByGameId(gameId);
		if (accountGameList.FirstOrDefault() == null) // NOT FOUND
		{
			return BadRequest(new
			{
				success = false,
				message = "That Game doesn't associate with any Account."
			});
		}



		return Ok(new
		{
			success = true,
			message = "Found!",
			data = accountGameList
		});
	}

	[HttpGet("GetAccountGameByAccountId/{accountId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByAccountId(int accountId)
	{
		var accountGameList = await _accountGameService.GetAccountGameByAccountId(accountId);
		if (accountGameList == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account doesn't associate with any Game."
			});
		}

		return Ok(new
		{
			success = true,
			message = "Found!",
			data = accountGameList
		});
	}

	[HttpPost("AddAccountGame")]
	public async Task<ActionResult<AccountGameFormModel>> AddAccountGame([FromBody] AccountGameFormModel accountGameFormModel)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
			});
		}

		// Retrieve both Account, Game and AccountGame data
		var checkAccount = await _accountService.GetAccountById(accountGameFormModel.AccountId);
		var checkGame = await _gameService.GetGameById(accountGameFormModel.GameId);

		// Check if both Account and game exist 
		if (checkAccount == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Account is not found!"
			});
		}

		if (checkGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game is not found!"
			});
		}

		// Check if that Account already has that game
		var checkAccountGame = (await _accountGameService.GetAccountGameByAccountId(accountGameFormModel.AccountId))
			.Where(g => g.GameId == checkGame.GameId);

		if (checkAccountGame.Any())
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account already has that Game."
			});
		}
		

		// Create new Account Game
		var accountGame = new AccountGame
		{
			AccountId = checkAccount.AccountId,
			GameId = checkGame.GameId,
			DateAdded = DateTime.UtcNow,
		};

		await _accountGameService.AddAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "Successfully added AccountGame",
			data = accountGame,
		});

	}

	[HttpPut("UpdateAccountGame/{accountGameId}")]
	public async Task<ActionResult> UpdateAccountGame(int accountGameId, [FromBody] AccountGameFormModel accountGameFormModel)
	{
		// Validate user input
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
			});
		}

		var checkAccountGame = await _accountGameService.GetAccountGameById(accountGameId);
		if (checkAccountGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "AccountGame not found"
			});
		}

		// Check if that Account already has that game (excluding the current accountGameId)
		var existingGames = (await _accountGameService.GetAccountGameByAccountId(accountGameFormModel.AccountId))
			.Where(g => g.GameId == accountGameFormModel.GameId && g.AccountGameId != accountGameId);

		if (existingGames.Any())
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account already has that Game."
			});
		}

		// Rest of your existing update logic...
		var accountGame = new AccountGame
		{
			AccountGameId = accountGameId,
			AccountId = accountGameFormModel.AccountId,
			GameId = accountGameFormModel.GameId,
			DateAdded = DateTime.UtcNow,
		};

		await _accountGameService.UpdateAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "AccountGame updated successfully",
			data = accountGame
		});
	}

	[HttpDelete("DeleteAccountGame/{accountGameId}")]
	public async Task<ActionResult> DeleteAccountGame(int accountGameId)
	{
		var accountGame = await _accountGameService.GetAccountGameById(accountGameId);
		if (accountGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "AccountGame do not exist or already deleted."
			});
		}

		await _accountGameService.DeleteAccountGame(accountGameId);
		return Ok(new
		{
			success = true,
			message = "AccountGame Successfully Deleted."
		});
	}
}
