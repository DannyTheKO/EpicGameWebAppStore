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

    #region Temp
    public async Task<IEnumerable<Cartdetail>> GetAllCartdetailsAsync()
    {
	    try
	    {
		    return await _cartdetailRepository.GetAll();
	    }
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to get all the cart details", ex);
	    }
    }

    public async Task<Cartdetail> AddCartdetailAsync(Cartdetail cartdetail)
    {
	    try
	    {
		    await _cartdetailRepository.Create(cartdetail);
		    return cartdetail;
	    }
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to add the cart detail", ex);
	    }
    }

    public async Task<Cartdetail> UpdateCartdetailAsync(Cartdetail cartdetail)
    {
	    try
	    {
		    await _cartdetailRepository.Update(cartdetail);
		    return cartdetail;
	    }
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to update the cart detail", ex);
	    }
    }

    public async Task<Cartdetail> DeleteCartdetailAsync(int id)
    {
	    var cartdetail = await _cartdetailRepository.GetById(id);
	    if (cartdetail == null) throw new Exception("Cart detail not found.");
	    await _cartdetailRepository.Delete(id);
	    return cartdetail;
    }

    public async Task<Cartdetail> GetCartdetailByIdAsync(int id)
    {
	    try
	    {
		    return await _cartdetailRepository.GetById(id);
	    }
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to get specific cart detail id", ex);
	    }
    }

    public async Task<IEnumerable<Cartdetail>> GetCartdetailsByCartIdAsync(int cartId)
    {
	    try
	    {
		    return await _cartdetailRepository.GetByCartId(cartId);
	    }
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to get cart details for the specified cart", ex);
	    }
    }
    

    #endregion

    #region Service

    //public int GetTotalAmount(int cartDetailId)
    //{

    //}

    #endregion
}