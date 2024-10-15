using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Domain
using EpicGameWebAppStore.Domain.Entities;

namespace EpicGameWebAppStore.Domain.Repository
{
    public interface IDiscountRepository
    {
        // Basic CRUD Function
        public Task<IEnumerable<Discount>> GetAll();
        public Task Add(Discount discount);
        public Task Update(Discount discount);
        public Task Delete(int id);


        // == Feature Function ==

        // Search By Discount ID
        public Task<Discount> GetById(int id);

        // Search By Game Discount => Get Game "ID"
    }
}
