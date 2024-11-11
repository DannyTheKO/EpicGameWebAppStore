<<<<<<< HEAD
﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Repository
{
    internal interface IGenreRepository
    {
        public Task<IEnumerable<Genre>> GetAll();
        public Task Add(Genre genre);
        public Task Update(Genre genre);
        public Task Delete(int id);


        // == Feature Function ==

        // Search by Game ID
        public Task<Genre> GetById(int id);
    }
}
=======
﻿// Domain

using Domain.Entities;

namespace Domain.Repository;

public interface IGenreRepository
{
    // == Basic CRUD Function ==
    public Task<IEnumerable<Genre>> GetAll();
    public Task Add(Genre genre);
    public Task Update(Genre genre);
    public Task Delete(int id);

    // == Feature Function ==

    // Search by Genre ID
    public Task<Genre> GetById(int id);

    // TODO: Search By Name
}
>>>>>>> Danny's_Branch
