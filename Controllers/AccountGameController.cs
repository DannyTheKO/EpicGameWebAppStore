using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
	public class AccountGameController : Controller
	{
		private readonly IAccountGameService _accountGameService;

		public AccountGameController(IAccountGameService accountGameService)
		{
			_accountGameService = accountGameService;
		}

		public async Task<ActionResult<AccountGame>> AddAccountGame(AccountGame accountGame)
		{
			var newAccountGame = await _accountGameService.AddAccountGame(accountGame);
			return Ok(newAccountGame);
		}

	}
}
