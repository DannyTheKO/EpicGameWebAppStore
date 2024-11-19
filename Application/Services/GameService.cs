using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Org.BouncyCastle.Asn1.X509;

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
		// Retrieve game from database
		var existingGame = await _gameRepository.GetById(game.GameId);
		if (existingGame == null) throw new Exception("Game not found.");

		// Update game into the database
		existingGame.GameId = game.GameId;
		existingGame.PublisherId = game.PublisherId;
		existingGame.GenreId = game.GenreId;
		existingGame.Title = game.Title;
		existingGame.Price = game.Price;
		existingGame.Author = game.Author;
		existingGame.Rating = game.Rating;
		existingGame.Description = game.Description;

		existingGame.Release = game.Release;
		existingGame.Publisher = game.Publisher;
		existingGame.Genre = game.Genre;

		// Call method to update
		await _gameRepository.Update(existingGame);
		return existingGame;
	}

	public async Task<Game> DeleteGame(int id)
	{
		var existingGame = await _gameRepository.GetById(id);
		if (existingGame == null) throw new Exception("Game not found.");
		await _gameRepository.Delete(id);
		return existingGame;
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
}
