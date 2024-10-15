using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

namespace EpicGameWebAppStore.Domain.Repository
{
    public interface IGameRepository
    {
        // == Basic CRUD Function ==
        public Task<IEnumerable<Game>> GetAll();
        public Task Add(Game game);
        public Task Update(Game game);
        public Task Delete(int id);


        // == Feature Function ==

        // Search by Game ID
        public Task<Game> GetById(int id);

        // TODO: Search By Publisher => Get Publisher By "ID"
        // TODO: Search By Genre => Get Genre By "ID"
        // TODO: Search By Name
        // TODO: Search By Publisher
        // TODO: Search By Rating
    }
}
