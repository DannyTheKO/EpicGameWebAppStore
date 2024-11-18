using Domain.Entities;

namespace Application.Interfaces;

public interface ICartdetailService
{
    Task<IEnumerable<Cartdetail>> GetAllCartDetails();
    Task<Cartdetail> GetCartDetailById(int id);
    Task<Cartdetail> AddCartDetail(Cartdetail cartDetail);
    Task<Cartdetail> UpdateCartDetail(Cartdetail cartDetail);
    Task<Cartdetail> DeleteCartDetail(int id);

    Task<IEnumerable<Cartdetail>> GetAllCartDetailByCartId(int cartId);
}