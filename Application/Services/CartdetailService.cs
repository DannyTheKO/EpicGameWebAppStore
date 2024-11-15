using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class CartdetailService : ICartdetailService
{
    private readonly ICartdetailRepository _cartdetailRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IGameRepository _gameRepository;

    public CartdetailService(ICartdetailRepository cartdetailRepository)
    {
        _cartdetailRepository = cartdetailRepository;
    }

	public async Task<IEnumerable<Cartdetail>> GetAllCartDetails()
	{
		return await _cartdetailRepository.GetAll();
	}

	public async Task<Cartdetail> AddCartDetail(Cartdetail cartDetail)
	{
		await _cartdetailRepository.Create(cartDetail);
		return cartDetail;
	}

	public async Task<Cartdetail> UpdateCartDetail(Cartdetail cartDetail)
	{
		await _cartdetailRepository.Update(cartDetail);
		return cartDetail;
	}

    public async Task<Cartdetail> DeleteCartDetail(int id)
    {
	    var cartdetail = await _cartdetailRepository.GetById(id);
	    if (cartdetail == null) throw new Exception("Cart detail not found.");
	    await _cartdetailRepository.Delete(id);
	    return cartdetail;
    }

	public async Task<Cartdetail> GetCartDetailById(int id)
	{
		return await _cartdetailRepository.GetById(id);
	}

	public async Task<IEnumerable<Cartdetail>> GetAllCartDetailByCartId(int cartId)
	{
		var listCartDetail = await _cartdetailRepository.GetAll();
		return listCartDetail.Where(x => x.CartId == cartId);
	}

}