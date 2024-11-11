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
public class GameController : _BaseController
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
        IAuthorizationServices authorizationServices,
        IAccountService accountService,
        IRoleService roleService)
        : base(authenticationServices, authorizationServices, accountService, roleService)
    {
        _gameServices = gameServices;
        _genreService = genreService;
        _publisherService = publisherService;
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

//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Application.Interfaces;
//using Domain.Entities;
//using Application.Services;

//namespace Presentation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GameController : ControllerBase
//    {
//        private readonly GameService _gameServices;

//        public GameController(GameService gameServices)
//        {
//            _gameServices = gameServices;
//        }

//        // GET: api/Game
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
//        {
//            var games = await _gameServices.GetAllGameAsync();
//            return Ok(games);
//        }

//        // GET: api/Game/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Game>> GetGameById(int id)
//        {
//            var game = await _gameServices.GetGameByIdAsync(id);
//            if (game == null)
//            {
//                return NotFound("Game not found.");
//            }
//            return Ok(game);
//        }

//        // POST: api/Game
//        [HttpPost]
//        public async Task<ActionResult<Game>> AddGame(Game game)
//        {
//            var createdGame = await _gameServices.AddGameAsync(game);
//            return CreatedAtAction(nameof(GetGameById), new { id = createdGame.GameId }, createdGame);
//        }

//        // PUT: api/Game/{id}
//        [HttpPut("{id}")]
//        public async Task<ActionResult<Game>> UpdateGame(int id, Game game)
//        {
//            if (id != game.GenreId)
//            {
//                return BadRequest("Game ID mismatch.");
//            }
//            var updatedGame = await _gameServices.UpdateGameAsync(game);
//            return Ok(updatedGame);
//        }

//        // DELETE: api/Game/{id}
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<Game>> DeleteGame(int id)
//        {
//            Console.WriteLine(id);
//            var deletedGame = await _gameServices.DeleteGameAsync(id);
//            return Ok(deletedGame);
//        }
//    }
//}
