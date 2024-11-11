using Domain.Entities;

namespace Application.Interfaces;

public interface ICartdetailService
{
    Task<IEnumerable<Cartdetail>> GetAllCartdetails();
    Task<Cartdetail> AddCartdetail(Cartdetail cartdetail);
    Task<Cartdetail> UpdateCartdetail(Cartdetail cartdetail);
    Task<Cartdetail> DeleteCartdetail(int id);
    Task<Cartdetail> GetCartdetailById(int id);
    Task<IEnumerable<Cartdetail>> GetCartdetailsByCartIdAsync(int cartId);
}