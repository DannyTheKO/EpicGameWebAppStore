using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

<<<<<<< HEAD
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
=======
namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class GenreController : Controller
{
	private readonly IGenreService _genreService;

	public GenreController(IGenreService genreService)
	{
		_genreService = genreService;
	}

	//GET: Genre/GetAllGenre
	[HttpGet("GetAllGenre")]
	public async Task<ActionResult<IEnumerable<Genre>>> GetAll()
	{
		var genre = await _genreService.GetAllGenres();
		return Ok(genre);
	}

	// GET: Genre/GetGenre/{genreId}
	[HttpGet("GetGenre/{genreId}")]
	public async Task<ActionResult<Genre>> GetGenre(int genreId)
	{
		// Check if Genre is existed
		var checkGenre = await _genreService.GetGenreById(genreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Genre do not exist!"
			});
		}

		return Ok(new
		{
			success = true,
			data = checkGenre
		});
	}

	// POST: Genre/CreateGenre
	[HttpPost("CreateGenre")]
	public async Task<ActionResult> CreateGenre([FromBody] GenreFormModel genreFormModel)
	{
		// Check if Genre already in the database
		var checkGenre = await _genreService.GetGenreByName(genreFormModel.Name);
		if (checkGenre.SingleOrDefault() != null) // FOUND
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre is already existed."
			});
		}

		// Create Genre
		var genre = new Genre()
		{
			Name = genreFormModel.Name
		};

		await _genreService.AddGenre(genre);
		return Ok(new
		{
			success = true,
			message = "Genre is successfully added",
			data = genre
		});

	}

	// PUT: Genre/UpdateGenre/{genreId}
	[HttpPut("UpdateGenre/{genreId}")]
	public async Task<ActionResult> UpdateGenre([FromBody] GenreFormModel genreFormModel, int genreId)
	{
		var checkGenre = await _genreService.GetGenreById(genreId);

		// Check if genre ID exist
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Genre do not exist!"
			});
		}

		// Check if genre name exist
		var checkGenreName = await _genreService.GetGenreByName(genreFormModel.Name);
		if (checkGenreName.SingleOrDefault() != null) // FOUND
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre name already exists!"
			});
		}

		// Create new Genre
		var genre = new Genre()
		{
			GenreId = checkGenre.GenreId,
			Name = genreFormModel.Name,
		};

		await _genreService.UpdateGenre(genre);
		return Ok(new
		{
			success = true,
			message = "Successfully Updated Genre",
			data = genre
		});
	}

	// DELETE: Genre/DeleteGenre/{genreId}
	[HttpDelete("DeleteGenre/{genreId}")]
	public async Task<ActionResult> DeleteGenre(int genreId)
	{
		var checkGenre = await _genreService.GetGenreById(genreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				sucess = false,
				message = "Request Genre already delete or not exist!"
			});
		}

		await _genreService.DeleteGenre(genreId);
		return Ok(new
		{
			success = true,
			message = "Genre Successfully Deleted."
		});
	}
}
>>>>>>> 7bc7d2dd36cb49ea71fba6fcc44270bff1903677
