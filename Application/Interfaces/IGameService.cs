using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IGameService
{
    // == Basic CRUD Function ==
    Task<IEnumerable<Game>> GetAllGame();
    Task<Game> AddGame(Game game);
    Task<Game> UpdateGame(Game game);
    Task<Game> DeleteGame(int id);

	// Search Game by Game ID
	Task<Game> GetGameById(int id);
    public Task<IEnumerable<Game>> GetTopTrendingGames(int count);

    public Task<IEnumerable<Game>> GetTopNewReleases(int count);
    // Get Game By Publisher ID
    Task<IEnumerable<Game>> GetGameByPublisherId(int publisherId);

	// Search By Genre => Get Genre By "ID"
	Task<IEnumerable<Game>> GetGameByGenreId(int genreId);

	// Search By Name
	Task<IEnumerable<Game>> GetGameByTitle(string name);

	// Search By Publisher
	Task<IEnumerable<Game>> GetGameByPublisher(string publisher);

	// Search By Rating
	Task<IEnumerable<Game>> GetGameByRating(int rating);
}