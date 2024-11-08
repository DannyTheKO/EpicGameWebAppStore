using Domain.Entities;

// Domain

namespace Application.Interfaces;

public interface IGameService
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Game>> GetAllGameAsync();
    public Task<Game> AddGameAsync(Game game);
    public Task<Game> UpdateGameAsync(Game game);
    public Task<Game> DeleteGameAsync(int id);

    // == Feature Function ==

    // Search by Game ID
    public Task<Game> GetGameByIdAsync(int id);

    // TODO: Search By Publisher => Get Publisher By "ID"
    // TODO: Search By Genre => Get Genre By "ID"
    // TODO: Search By Name
    // TODO: Search By Publisher
    // TODO: Search By Rating
}