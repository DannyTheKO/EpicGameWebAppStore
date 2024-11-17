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
	    try
	    {
		return await _cartdetailRepository.GetAll();
	}
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to get all the cart details", ex);
	    }
    }

	public async Task<Cartdetail> AddCartDetail(Cartdetail cartDetail)
	{
		await _cartdetailRepository.Create(cartDetail);
		return cartDetail;
	}
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to add the cart detail", ex);
	    }
    }

	public async Task<Cartdetail> UpdateCartDetail(Cartdetail cartDetail)
	{
		await _cartdetailRepository.Update(cartDetail);
		return cartDetail;
	}
	    catch (Exception ex)
	    {
		    throw new Exception("Failed to update the cart detail", ex);
	    }
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


	#region Service

	// Calculate single session detail cart
	public async Task<(decimal TotalPrice, bool Flag)> GetTotalAmount(int cartDetailId)
	{
		//Check if that cartDetail available
		var existingCartDetail = await _cartdetailRepository.GetByCartId(cartDetailId);
		if (existingCartDetail == null) // NOT FOUND
		{
			return (0, false);
		}

		var totalPrice = existingCartDetail
			.Sum(cd => cd.Price.GetValueOrDefault() * cd.Quantity.GetValueOrDefault() * 
			           (1 - cd.Discount.GetValueOrDefault() / 100m));
		
		return (totalPrice, true);
	}

	#endregion
}