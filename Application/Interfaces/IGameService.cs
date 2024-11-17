using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IGameService
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Game>> GetAllGame();
    public Task<Game> AddGame(Game game);
    public Task<Game> UpdateGame(Game game);
    public Task<Game> DeleteGame(int id);
    // == Basic CRUD Function ==
    Task<IEnumerable<Game>> GetAllGame();
    Task<Game> AddGame(Game game);
    Task<Game> UpdateGame(Game game);
    Task<Game> DeleteGame(int id);

	// Search Game by Game ID
	Task<Game> GetGameById(int id);

	// Get Game By Publisher ID
	Task<IEnumerable<Game>> GetGameByPublisherId(int publisherId);

	// Search By Genre => Get Genre By "ID"
	Task<IEnumerable<Game>> GetGameByGenreId(int genreId);

    // == Feature Function ==
	// Search By Name
	Task<IEnumerable<Game>> GetGameByTitle(string name);

    // Search by Game ID
    public Task<Game> GetGameById(int id);
    public Task<IEnumerable<Game>> GetTopTrendingGames(int count);
	// Search By Publisher
	Task<IEnumerable<Game>> GetGameByPublisher(string publisher);

    public Task<IEnumerable<Game>> GetTopNewReleases(int count);
    // TODO: Search By Publisher => Get Publisher By "ID"
    // TODO: Search By Genre => Get Genre By "ID"
    // TODO: Search By Name
    // TODO: Search By Publisher
    // TODO: Search By Rating
	// Search By Rating
	Task<IEnumerable<Game>> GetGameByRating(int rating);
}