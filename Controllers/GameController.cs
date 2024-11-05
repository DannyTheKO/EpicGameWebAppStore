using System.Security.Claims;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator, Editor")]
public class GameController : _BaseController
{
	private readonly IGameService _gameServices;
	private readonly IAuthorizationServices _authorizationServices;

	public GameController(IGameService gameServices, IAuthenticationServices authenticationServices, IAuthorizationServices authorizationServices)
		: base(authenticationServices, authorizationServices)
	{
		_gameServices = gameServices;
		_authorizationServices = authorizationServices;
	}

	// GET: Game/Index
	public async Task<IActionResult> Index()
	{
		var games = await _gameServices.GetAllGameAsync();
		return View(games);
	}

	// TODO: Create Function
	// GET: Game/Create
	public async Task<IActionResult> Create()
	{
		bool hasPermission = await _authorizationServices.UserHasPermission(GetCurrentLoginAccountId(), "delete");

		if (!hasPermission) // Dont have permission of that role
		{
			return RedirectToAction("AccessDenied", "Auth");
		}

		return View();
	}

	// POST: Game/Create

	// TODO: Update Function 
	// GET: Game/Update/{id}
	// POST: Game/Update/{id}

	// TODO: Delete Function
	// GET: Game/Delete/{id}
	// POST: Game/Delete/{id}
}
