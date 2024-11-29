using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class GenreController : _BaseController
{
    private readonly IGenreService _genreService;
    private readonly IAuthorizationServices _authorizationServices;

    public GenreController(IGenreService genreService, IAuthorizationServices authorizationServices) : base(authorizationServices)
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
