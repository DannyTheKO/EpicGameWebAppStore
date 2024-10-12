using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

namespace EpicGameWebAppStore.Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetGameByIdAsync(int id);

        Task<IEnumerable<Game>> GetAllGameAsync();
        
        Task AddGameAsync(Game game);
        
        Task UpdateGameAsync(Game game);

        Task DeleteGameAsync(int id);
    }
}
