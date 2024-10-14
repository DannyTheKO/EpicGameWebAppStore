using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

// Application


namespace EpicGameWebAppStore.Application.Interfaces
{
    public interface IGameServices
    {
        public Task<Game> GetGameByIdAsync(int id);
        public Task<IEnumerable<Game>> GetAllGameAsync();
        public Task<Game> AddGameAsync(Game game);
        public Task<Game> UpdateGameAsync(Game game);
        public Task<Game> DeleteGameAsync(int id);
    }
}
