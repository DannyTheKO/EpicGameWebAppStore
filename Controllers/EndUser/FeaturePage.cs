using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.EndUser;


[ApiController]
[Route("Store/[controller]")]
public class FeaturePage : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;
	private readonly IGameService _gameServices;

	public FeaturePage(
		IAuthenticationServices authenticationServices,
		IAuthorizationServices authorizationServices,
		IGameService gameServices)
		: base(authorizationServices)
	{
		_authenticationServices = authenticationServices;
		_authorizationServices = authorizationServices;
		_gameServices = gameServices;
	}

	// GET: Game/Index
	[HttpGet("GetAll")]
	public async Task<ActionResult<IEnumerable<Game>>> GetAll()
	{
		var games = await _gameServices.GetAllGame();
		return Ok(games);
	}

	// GET: Game/GetTrendingGames
	[HttpGet("GetTrendingGames")]
	public async Task<ActionResult<IEnumerable<Game>>> GetTrendingGames()
	{
		var games = await _gameServices.GetAllGame();
		var trendingGames = games
			.Where(g => g.Rating != null) // Chỉ lấy game có Rating hợp lệ
			.OrderByDescending(g => g.Rating) // Sắp xếp theo Rating cao nhất
			.Take(5) // Giới hạn 5 game
			.ToList();

		if (!trendingGames.Any()) // Nếu không có game
			return NotFound(new { message = "No trending games available." });

		return Ok(trendingGames);
	}

	// GET: Game/GetTopNewReleases
	[HttpGet("GetTopNewReleases")]
	public async Task<ActionResult<IEnumerable<Game>>> GetTopNewReleases()
	{
		var topNewReleases = await _gameServices.GetTopNewReleases(10); // Lấy 10 game mới phát hành
		return Ok(topNewReleases);
	}
}