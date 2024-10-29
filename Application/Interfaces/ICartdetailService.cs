using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICartdetailService
    {
        Task<IEnumerable<Cartdetail>> GetAllCartdetailsAsync();
        Task<Cartdetail> AddCartdetailAsync(Cartdetail cartdetail);
        Task<Cartdetail> UpdateCartdetailAsync(Cartdetail cartdetail);
        Task<Cartdetail> DeleteCartdetailAsync(int id);
        Task<Cartdetail> GetCartdetailByIdAsync(int id);
        Task<IEnumerable<Cartdetail>> GetCartdetailsByCartIdAsync(int cartId);
    }
}
