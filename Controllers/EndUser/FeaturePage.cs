using Application.Interfaces;
using Application.Services;
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
    private readonly IGenreService _genreService;
    private readonly IPublisherService _publisherService; 
    public FeaturePage(
		IAuthenticationServices authenticationServices,
		IAuthorizationServices authorizationServices,
		IGameService gameServices , IGenreService genreService , IPublisherService publisherService)
		: base(authorizationServices)
	{
		_authenticationServices = authenticationServices;
		_authorizationServices = authorizationServices;
		_gameServices = gameServices;
        _genreService = genreService;
        _publisherService = publisherService; ;
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
    //GET: Genre/GetAllGenre
    [HttpGet("GetAllGenre")]
    public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenre()
    {
        var genre = await _genreService.GetAllGenres();
        return Ok(genre);
    }
    // Featured Publishers API
    [HttpGet("GetFeaturedPublishers")]
    public async Task<ActionResult<IEnumerable<object>>> GetFeaturedPublishers()
    {
        var publishers = await _publisherService.GetAllPublishers();

        var featuredPublishers = publishers
            .Select(p => new
            {
                PublisherId = p.PublisherId,
                Name = p.Name,
                Address = p.Address,
                Email = p.Email,
                Phone = p.Phone,
                Website = p.Website,
                GameCount = _gameServices.GetGameByPublisherId(p.PublisherId).Result.Count(),
                Games = _gameServices.GetGameByPublisherId(p.PublisherId).Result
                    .Take(4) // Lấy tối đa 4 game
                    .Select(g => new
                    {
                        GameId = g.GameId,
                        Title = g.Title,
                        Price = g.Price,
                        ImageGame = g.ImageGame
                    })
            })
            .OrderByDescending(p => p.GameCount)
            .Take(3)
            .ToList();

        if (!featuredPublishers.Any())
            return NotFound(new { message = "No featured publishers available." });

        return Ok(featuredPublishers);
    }

    // Top Genres API
    [HttpGet("GetTopGenres")]
    public async Task<ActionResult<IEnumerable<object>>> GetTopGenres()
    {
        var allGenres = await _genreService.GetAllGenres();
        var games = await _gameServices.GetAllGame();

        var topGenres = allGenres
            .Select(genre => new
            {
                GenreId = genre.GenreId,
                Name = genre.Name,
                GameCount = games.Count(g => g.GenreId == genre.GenreId),
                Games = games.Where(g => g.GenreId == genre.GenreId)
                    .OrderByDescending(g => g.Rating) // Sắp xếp theo Rating cao nhất trước
                    .Take(4) // Lấy tối đa 3 game
                    .Select(g => new
                    {
                        GameId = g.GameId,
                        Title = g.Title,
                        Price = g.Price,
                        ImageGame = g.ImageGame,
                        Rating = g.Rating // Hiển thị Rating của game
                    })
            })
            .OrderByDescending(g => g.GameCount) // Giữ nguyên sắp xếp theo số lượng game
            .Take(3)
            .ToList();

        return Ok(topGenres);
    }

    [HttpGet("GetTopFreeToPlay")]
    public async Task<ActionResult<IEnumerable<object>>> GetTopFreeToPlay()
    {
        var games = await _gameServices.GetAllGame();

        var freeToPlayGames = games
            .Where(g => g.Price == 0) // Chỉ lấy các game miễn phí và giá là 0
            .Take(5) // Lấy 5 game
            .Select(g => new
            {
                GameId = g.GameId,
                Title = g.Title,
                Price = "Free", // Giá là "Free" nếu miễn phí và giá 0
                ImageGame = g.ImageGame
            })
            .ToList();

        if (!freeToPlayGames.Any()) // Nếu không có game
            return NotFound(new { message = "No free-to-play games available." });

        return Ok(freeToPlayGames);
    }


}