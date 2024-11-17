// Domain

using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

// Application

namespace Application.Services;

public class GameService : IGameService
{
    // Create Constructor
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    // == Basic CRUD Function ==
    public async Task<IEnumerable<Game>> GetAllGame()
    {
        try
        {
            return await _gameRepository.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get all the games", ex);
        }
    }

    public async Task<Game> AddGame(Game game)
    {
        try
        {
            await _gameRepository.Add(game);
            return game;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add the game", ex);
        }
    }

    public async Task<Game> UpdateGame(Game game)
    {
        try
        {
            await _gameRepository.Update(game);
            return game;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update the game", ex);
        }
    }

    public async Task<Game> DeleteGame(int id)
    {
        var game = await _gameRepository.GetById(id);
        if (game == null) // Not Found
            throw new Exception("Game not found.");
        await _gameRepository.Delete(id);
        return game;
    }

    // == Feature Function ==

    // Search by Game ID
    public async Task<Game> GetGameById(int id)
    {
        try
        {
            return await _gameRepository.GetById(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get specific game id", ex);
        }
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
    // TODO: Search By Publisher => Get Publisher By "ID"
    // TODO: Search By Genre => Get Genre By "ID"
    // TODO: Search By Name
    // TODO: Search By Publisher
    // TODO: Search By Rating
}