using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

namespace EpicGameWebAppStore.Application.Interfaces
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetAll();
        public Task<Game> GetById(int id);
        public Task Add(Game game);
        public Task Update(Game game);
        public Task Delete(int id);
    }
}
