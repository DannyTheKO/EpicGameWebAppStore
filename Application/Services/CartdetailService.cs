using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class CartdetailService : ICartdetailService
{
    private readonly ICartdetailRepository _cartdetailRepository;

    public CartdetailService(ICartdetailRepository cartdetailRepository)
    {
        _cartdetailRepository = cartdetailRepository;
    }

    public async Task<IEnumerable<Cartdetail>> GetAllCartdetails()
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

    public async Task<Cartdetail> AddCartdetail(Cartdetail cartdetail)
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

    public async Task<Cartdetail> UpdateCartdetail(Cartdetail cartdetail)
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

    public async Task<Cartdetail> DeleteCartdetail(int id)
    {
        var cartdetail = await _cartdetailRepository.GetById(id);
        if (cartdetail == null) throw new Exception("Cart detail not found.");
        await _cartdetailRepository.Delete(id);
        return cartdetail;
    }

    public async Task<Cartdetail> GetCartdetailById(int id)
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
}