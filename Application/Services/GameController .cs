using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
        {
            var games = await _gameServices.GetAllGameAsync();
            return Ok(games);
        }

        // GET: api/Game/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(int id)
        {
            var game = await _gameServices.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            return Ok(game);
        }

        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<Game>> AddGame(Game game)
        {
            var createdGame = await _gameServices.AddGameAsync(game);
            return CreatedAtAction(nameof(GetGameById), new { id = createdGame.GameId }, createdGame);
        }

        // PUT: api/Game/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> UpdateGame(int id, Game game)
        {
            if (id != game.GenreId)
            {
                return BadRequest("Game ID mismatch.");
            }
            var updatedGame = await _gameServices.UpdateGameAsync(game);
            return Ok(updatedGame);
        }

        // DELETE: api/Game/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var deletedGame = await _gameServices.DeleteGameAsync(id);
            return Ok(deletedGame);
        }
    }
}
