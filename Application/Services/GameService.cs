using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

// Application

namespace Application.Services;

public class GameService : IGameService
{
	private readonly IGameRepository _gameRepository;

	public GameService(IGameRepository gameRepository)
	{
		_gameRepository = gameRepository;
	}

	// == Basic CRUD Function ==
	public async Task<IEnumerable<Game>> GetAllGame()
	{
		return await _gameRepository.GetAll();
	}

	public async Task<Game> AddGame(Game game)
	{
		await _gameRepository.Add(game);
		return game;
	}

	public async Task<Game> UpdateGame(Game game)
	{
		await _gameRepository.Update(game);
		return game;
	}

	public async Task<Game> DeleteGame(int id)
	{
		var game = await _gameRepository.GetById(id);
		if (game == null) throw new Exception("Game not found.");
		await _gameRepository.Delete(id);
		return game;
	}

	// == Feature Function ==
	// Search name by Game ID
	public async Task<Game> GetGameById(int id)
	{
		return await _gameRepository.GetById(id);
	}

	// Search games by Publisher ID
	public async Task<IEnumerable<Game>> GetGameByPublisherId(int publisherId)
	{
		var gameList = await _gameRepository.GetAll();
		var filteredGame = gameList.Where(f => f.PublisherId == publisherId);
		return filteredGame;
	}

	// Search games by Genre ID
	public async Task<IEnumerable<Game>> GetGameByGenreId(int genreId)
	{
		var gameList = await _gameRepository.GetAll();
		var filteredGame = gameList.Where(f => f.GenreId == genreId);
		return filteredGame;
	}

	// Search games by Title
	public async Task<IEnumerable<Game>> GetGameByTitle(string title)
	{
		var gameList = await _gameRepository.GetAll();
		var filteredGame = gameList.Where(f => f.Title == title);
		return filteredGame;
	}

	// Search games by Publisher Name
	public async Task<IEnumerable<Game>> GetGameByPublisher(string publisher)
	{
		var gameList = await _gameRepository.GetAll();
		var filteredGame = gameList.Where(f => f.Publisher.Name == publisher);
		return filteredGame;
	}

	// Search games by Rating
	public async Task<IEnumerable<Game>> GetGameByRating(int rating)
	{
		var gameList = await _gameRepository.GetAll();
		var filteredGame = gameList.Where(f => f.Rating == rating);
		return filteredGame;
	}
    public async Task<IEnumerable<Game>> GetTopTrendingGames(int count)
    {
        try
        {
            var games = await _gameRepository.GetAll();
            return games
                .OrderByDescending(g => g.Rating) // Sắp xếp giảm dần theo Rating
                .Take(count); // Lấy số lượng cần thiết
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get top trending games", ex);
        }
    }

    // Lấy các game có ngày phát hành mới nhất
    public async Task<IEnumerable<Game>> GetTopNewReleases(int count)
    {
        try
        {
            var games = await _gameRepository.GetAll();
            return games
                .OrderByDescending(g => g.Release) // Sắp xếp giảm dần theo ReleaseDate
                .Take(count); // Lấy số lượng cần thiết
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get top new releases", ex);
        }
    }
}
