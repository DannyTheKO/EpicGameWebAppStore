using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

//[Authorize(Roles = "Admin, Moderator, Editor")]
[Route("[controller]")]
[ApiController]
public class GameController : Controller
{
    private readonly IAuthenticationServices _authenticationServices;
    private readonly IAuthorizationServices _authorizationServices;
    private readonly IGameService _gameServices;
    private readonly IGenreService _genreService;
    private readonly IPublisherService _publisherService;

    public GameController(
        IGameService gameServices,
        IGenreService genreService,
        IPublisherService publisherService,
        IAuthenticationServices authenticationServices,
        IAuthorizationServices authorizationServices)
    {
        _gameServices = gameServices;
        _genreService = genreService;
        _publisherService = publisherService;
        _authenticationServices = authenticationServices;
        _authorizationServices = authorizationServices;
    }
    // GET: Game/GetTrending
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
        try
        {
            var topNewReleases = await _gameServices.GetTopNewReleases(10); // Lấy 10 game mới phát hành
            return Ok(topNewReleases);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    // GET: Game/Index
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Game>>> GetAll()
    {
        var games = await _gameServices.GetAllGame();
        return Ok(games);
    }

    [HttpGet("GetGame/{gameId}")]
    public async Task<ActionResult<Game>> GetGameById([FromBody] int gameId)
	{
		var game = await _gameServices.GetGameById(gameId);
		if (game == null) return NotFound();
		return Ok(game);
	}

	// POST: Game/CreateConfirm
	[HttpPost("CreateGame")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
	            message = "Fail to add game"
            });
        }

        await _gameServices.AddGame(game);
        return Ok(new
        {
	        success = true,
	        message = "Successfully to add game"
        });
    }

    // PUT: Game/UpdateConfirm/{id}
    [HttpPut("UpdateGame/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<Game>> UpdateGame(Game game, int id)
    {
        // Check if that game ID user was looking for is available
        if (id != game.GameId) 
	        return BadRequest(new
        {
            success = false,
            message = "ID game don't match with the database or the game is updated"
        });

        // Check if the requirement is valid
        if (!ModelState.IsValid)
	        return BadRequest(new
	        {
		        success = false,
		        message = "Missing Input Requirement"
	        });

        await _gameServices.AddGame(game);
        return Ok(new
        {
	        success = true,
	        message = "Add Game Success"
        });
    }

    // DELETE: Game/DeleteConfirm/{id}
    [HttpDelete("DeleteConfirm/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
	    var existingGame = await _gameServices.GetGameById(id);
	    if (existingGame == null)
	    {
		    return BadRequest(new
		    {
			    success = false,
			    message = "ID game don't match with the database or the game is deleted"
		    });
	    }

	    await _gameServices.DeleteGame(id);
	    return Ok(new
	    {
		    success = false,
		    message = "Delete Game Success"
	    });
    }
}

