using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using EpicGameWebAppStore.Models;
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

	[HttpGet("GetGameId/{gameId}")]
	public async Task<ActionResult<Game>> GetGameById(int gameId)
	{
		var game = await _gameServices.GetGameById(gameId);
		if (game == null) return NotFound(new
		{
			success = false,
			message = "Requested game is not found!"
		});

		return Ok(game);
	}

	// POST: Game/CreateGame
	[HttpPost("CreateGame")]
<<<<<<< HEAD
    public async Task<ActionResult<Game>> CreateGame([FromBody] Game game)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
	            message = "Fail to add game"
            });
        }
=======
	public async Task<ActionResult> CreateGame([FromBody] GameFormModel gameFormModel)
	{
		// Validate User Input
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677

			});
		}

<<<<<<< HEAD
    // PUT: Game/UpdateConfirm/{id}
    [HttpPut("UpdateGame/{id}")]
    public async Task<ActionResult<Game>> UpdateGame([FromBody] Game game, int id)
    {
        // Check if that game ID user was looking for is available
        if (id != game.GameId) 
	        return BadRequest(new
        {
            success = false,
            message = "ID game don't match with the database or the game is updated"
        });

        // Check if the requirement is valid
       
=======
		// Check if Publisher ID is available
		var checkPublisher = await _publisherService.GetPublisherById(gameFormModel.PublisherId);
		if (checkPublisher == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Publisher ID not found"
			});
		}

		// Check if Genre ID is available
		var checkGenre = await _genreService.GetGenreById(gameFormModel.GenreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre ID not found"
			});
		}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677

		// Create a new game
		var game = new Game()
		{
			PublisherId = gameFormModel.PublisherId,
			GenreId = gameFormModel.GenreId,
			Title = gameFormModel.Title,
			Price = gameFormModel.Price,
			Author = gameFormModel.Author,
			Release = DateTime.UtcNow,
			Rating = gameFormModel.Rating,
			Description = gameFormModel.Description,
		};

<<<<<<< HEAD
    // DELETE: Game/DeleteConfirm/{id}
    [HttpDelete("DeleteConfirm/{id}")]

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
		    success = true,
		    message = "Delete Game Success"
	    });
    }
=======
		await _gameServices.AddGame(game);
		return Ok(new
		{
			success = true,
			message = "Successfully to add game",
			data = game
		});
	}

	// PUT: Game/UpdateGame/{gameId}
	[HttpPut("UpdateGame/{gameId}")]
	public async Task<ActionResult> UpdateGame(int gameId, [FromBody] GameFormModel gameFormModel)
	{
		// Check if user input is valid
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

		// Check if the game ID is existed in the database
		var checkGame = await _gameServices.GetGameById(gameId);
		if (checkGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game ID not found"
			});
		}

		// Check if the publisher ID is existed in the database
		var checkPublisher = await _publisherService.GetPublisherById(gameFormModel.PublisherId);
		if (checkPublisher == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Publisher ID not found"
			});
		}

		// Check if the genre ID is existed in the database
		var checkGenre = await _genreService.GetGenreById(gameFormModel.GenreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre ID not found"
			});
		}

		// Create new game
		var game = new Game()	
		{
			GameId = checkGame.GameId,
			PublisherId = gameFormModel.PublisherId,
			GenreId = gameFormModel.GenreId,
			Title = gameFormModel.Title,
			Price = gameFormModel.Price,
			Author = gameFormModel.Author,
			Rating = gameFormModel.Rating,
			Description = gameFormModel.Description,

			// Get detail from existing game
			Release = checkGame.Release.Value,
			Genre = checkGenre,
			Publisher = checkPublisher,
		};

		await _gameServices.UpdateGame(game);
		return Ok(new
		{
			success = true,
			message = "Update Game Successfully",
			data = game
		});
	}

	// DELETE: Game/DeleteGame/{gameId}
	[HttpDelete("DeleteGame/{gameId}")]
	public async Task<ActionResult> DeleteConfirmed(int gameId)
	{
		var existingGame = await _gameServices.GetGameById(gameId);
		if (existingGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "ID game don't match with the database or the game is deleted"
			});
		}

		await _gameServices.DeleteGame(gameId);
		return Ok(new
		{
			success = false,
			message = "Delete Game Success"
		});
	}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677
}

