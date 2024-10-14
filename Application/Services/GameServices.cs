using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

// Application
using EpicGameWebAppStore.Application.Interfaces;

namespace EpicGameWebAppStore.Application.Services
{
    public class GameServices : IGameServices
    {
        private readonly IGameRepository _gameRepository;

        public GameServices(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // TODO: Searching game by Name
        // TODO: Searching game by Genre
        // TODO: Searching game by Publisher
        // TODO: Searching game by Release Date ? not sure...
        // This is for searching game feature... for later use - Danny
        public async Task<Game> GetGameByIdAsync(int id)
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

        public async Task<IEnumerable<Game>> GetAllGameAsync()
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

        public async Task<Game> AddGameAsync(Game game)
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

        public async Task<Game> UpdateGameAsync(Game game)
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

        public async Task<Game> DeleteGameAsync(int id)
        {
            var game = await _gameRepository.GetById(id);
            if (game == null) // Not Found
            {
                throw new Exception("Game not found.");
            }
            await _gameRepository.Delete(id);
            return game;
        }
    }
}
