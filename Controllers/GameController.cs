using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Authorize(Roles = "Admin, Moderator")]
[Route("[controller]")]
[ApiController]
public class GameController : _BaseController
{
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;
	private readonly IGameService _gameServices;
	private readonly IGenreService _genreService;
	private readonly IPublisherService _publisherService;
	private readonly IImageGameService _imageGameService;

	public GameController(
		IGameService gameServices,
		IGenreService genreService,
		IPublisherService publisherService,
		IImageGameService imageGameService,
		IAuthenticationServices authenticationServices,
		IAuthorizationServices authorizationServices) : base(authorizationServices)
	{
		_gameServices = gameServices;
		_genreService = genreService;
		_publisherService = publisherService;
		_imageGameService = imageGameService;
		_authenticationServices = authenticationServices;
		_authorizationServices = authorizationServices;
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
	public async Task<ActionResult> CreateGame([FromForm] GameFormModel gameFormModel, IFormFile imageFile)
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

		// Check if Game name is already existed
		var checkGame = await _gameServices.GetGameByTitle(gameFormModel.Title);
		var existingGame = checkGame.FirstOrDefault();

		if (existingGame != null)
		{
			// If game exists and image is provided, add it to existing game
			if (imageFile != null)
			{
				var (imageGame, flag) = await _imageGameService.UploadImageGame(imageFile, existingGame.GameId);
				if (!flag)
				{
					return BadRequest(new
					{
						success = flag,
						message = "Failed to upload image",
					});
				}

				return Ok(new
				{
					success = true,
					message = "Image added to existing game successfully",
					data = new
					{
						game = existingGame,
						newImage = imageGame
					}
				});
			}

			return Ok(new
			{
				success = true,
				message = "Game already exists",
				data = existingGame
			});
		}

		// If game doesn't exist, create new game
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

		await _gameServices.AddGame(game);

		if (imageFile != null)
		{
			var (imageGame, flag) = await _imageGameService.UploadImageGame(imageFile, game.GameId);
			if (!flag)
			{
				return BadRequest(new
				{
					success = flag,
					message = "Failed to upload image",
				});
			}

			game.ImageId = imageGame.ImageId;
		}

		await _gameServices.UpdateGame(game);

		return Ok(new
		{
			success = true,
			message = "Successfully added new game",
			data = game
		});
	}

	// PUT: Game/UpdateGame/{gameId}
	[HttpPut("UpdateGame/{gameId}")]
	public async Task<ActionResult> UpdateGame([FromBody] GameFormModel gameFormModel, int gameId)
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
			Release = gameFormModel.Release,

			// Get detail from existing game
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
}
