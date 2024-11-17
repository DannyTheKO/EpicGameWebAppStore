using Domain.Entities;

namespace Application.Interfaces;

public interface ICartdetailService
{
    Task<IEnumerable<Cartdetail>> GetAllCartDetails();
    Task<Cartdetail> AddCartDetail(Cartdetail cartDetail);
    Task<Cartdetail> UpdateCartDetail(Cartdetail cartDetail);
    Task<Cartdetail> DeleteCartDetail(int id);
    Task<Cartdetail> GetCartDetailById(int id);
}