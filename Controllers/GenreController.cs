using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // == Get All Genres ==
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllGenres()
        {
            try
            {
                var genres = await _genreService.GetAllGenres();
                return Ok(genres);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve genres", details = ex.Message });
            }
        }

        // == Get Genre by ID ==
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            try
            {
                var genre = await _genreService.GetGenreById(id);
                if (genre == null)
                    return NotFound(new { message = "Genre not found." });

                return Ok(genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve genre by ID", details = ex.Message });
            }
        }

        // == Add a Genre ==
        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] Genre genre)
        {
            if (genre == null)
                return BadRequest(new { message = "Invalid genre data." });

            try
            {
                var createdGenre = await _genreService.AddGenre(genre);
                return CreatedAtAction(nameof(GetGenreById), new { id = createdGenre.GenreId }, createdGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add genre", details = ex.Message });
            }
        }

        // == Update a Genre ==
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] Genre genre)
        {
            if (genre == null || genre.GenreId != id)
                return BadRequest(new { message = "Genre ID mismatch or invalid data." });

            try
            {
                var updatedGenre = await _genreService.UpdateGenre(genre);
                return Ok(updatedGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update genre", details = ex.Message });
            }
        }

        // == Delete a Genre ==
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var deletedGenre = await _genreService.DeleteGenre(id);
                return Ok(deletedGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete genre", details = ex.Message });
            }
        }
    }
}
