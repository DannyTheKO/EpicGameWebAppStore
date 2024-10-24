using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
// Application

// Domain


namespace EpicGameWebAppStore.Controllers;

public class GameController : Controller
{
	private readonly IGameService _gameServices;

	public GameController(IGameService gameServices)
	{
		_gameServices = gameServices;
	}

	// GET: Game/Index
	public async Task<IActionResult> Index()
	{
		var games = await _gameServices.GetAllGameAsync();
		return View(games);
	}

	// TODO: Create Function
	// GET: Game/Create
	public IActionResult Create()
	{
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