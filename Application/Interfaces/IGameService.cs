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

    // == Feature Function ==

    // Search by Game ID
    public Task<Game> GetGameById(int id);
    public Task<IEnumerable<Game>> GetTopTrendingGames(int count);

    public Task<IEnumerable<Game>> GetTopNewReleases(int count);
    // TODO: Search By Publisher => Get Publisher By "ID"
    // TODO: Search By Genre => Get Genre By "ID"
    // TODO: Search By Name
    // TODO: Search By Publisher
    // TODO: Search By Rating
}