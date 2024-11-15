﻿using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountGameController : Controller
{
	private readonly IAccountGameService _accountGameService;

	public AccountGameController(IAccountGameService accountGameService)
	{
		_accountGameService = accountGameService;
	}

	[HttpGet("AddAccountGame")]
	public async Task<ActionResult<AccountGame>> AddAccountGame([FromBody] AccountGame accountGame)
	{
		await _accountGameService.AddAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "AccountGame added successfully",
		});
	}

	[HttpGet("GetAllAccountGame")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAllAccountGame()
	{
		var accountGameList = await _accountGameService.GetAllAccountGame();
		return Ok(accountGameList);
	}

	[HttpPost("UpdateAccountGame/{accountId}")]
	public async Task<ActionResult> UpdateAccountGame([FromQuery] int accountId, [FromBody] AccountGame accountGame)
	{
		var getAccountGame = await _accountGameService.GetAccountGameByAccountId(accountId);

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

		if (getAccountGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "AccountGame not found"
			});
		}


		await _accountGameService.UpdateAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "AccountGame updated successfully",
			getAccountGame
		});
	}

	[HttpGet("GetAccountGameByGameId/{gameId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByGameId([FromBody] int gameId)
	{
		var accountGameList = await _accountGameService.GetAccountGameByGameId(gameId);

		if (accountGameList == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Game do not exist!"
			});
		}

		return Ok(new
		{
			success = true,
			message = "Game found",
			accountGameList
		});
	}

	[HttpGet("GetAccountGameByAccountId/{accountId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByAccountId([FromBody] int accountId)
	{
		var accountGameList = await _accountGameService.GetAccountGameByAccountId(accountId);
		
		if (accountGameList == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Account do not exist!"
			});
		}

		return Ok(new
		{
			success = true,
			message = "Account found",
			accountGameList
		});
	}
}