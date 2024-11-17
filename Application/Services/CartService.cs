using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;

namespace Application.Services;

public class CartService : ICartService
{
	private readonly ICartRepository _cartRepository;

	public CartService(ICartRepository cartRepository)
	{
		_cartRepository = cartRepository;
	}

	#region == CRUB Operation ==

	public async Task<IEnumerable<Cart>> GetAllCarts()
	{
		return await _cartRepository.GetAll();
	}

	public async Task<Cart> AddCart(Cart cart)
	{
		await _cartRepository.Add(cart);
		return cart;
	}

	public async Task<Cart> UpdateCart(Cart cart)
	{
		await _cartRepository.Update(cart);
		return cart;
	}

	public async Task<Cart> DeleteCart(int id)
	{
		var cart = await _cartRepository.GetById(id);
		if (cart == null) throw new Exception("Cart not found.");
		await _cartRepository.Delete(id);
		return cart;
	}

	public async Task<Cart> GetCartById(int id)
	{
		return await _cartRepository.GetById(id);
	}

	public async Task<IEnumerable<Cart>> GetCartsByAccountId(int accountId)
	{
		return await _cartRepository.GetAllCartFromAccountId(accountId);
	}

	public async Task<string> GetAccountByCartId(int accountId)
	{
		var account = await _cartRepository.GetAccountByCartId(accountId);
		return account?.Username ?? throw new Exception("Account not found.");
	}

	public async Task<string> GetPaymentMethodNameById(int paymentMethodId)
	{
		var paymentMethod = await _cartRepository.GetPaymentMethodById(paymentMethodId);
		return paymentMethod?.Name ?? throw new Exception("Payment method not found.");
	}

	#endregion


	public async Task<Cart> AddToCart(int accountId, int gameId, int quantity)
	{
		var existingCarts = await _cartRepository.GetAllCartFromAccountId(accountId);
		var existingCart = existingCarts.FirstOrDefault(); // Get most recent cart or create new one

		if (existingCart == null) // Cart doesn't exist for that account
		{
			// Create new cart
			var newCart = new Cart();
			newCart.InitializeCart(accountId);

			// Add new cart detail
			var cartDetail = new Cartdetail
			{
				GameId = gameId,
				Quantity = quantity
			};

			newCart.Cartdetails.Add(cartDetail);

			// Save to database
			await _cartRepository.Add(newCart);
			return newCart;
		}
		else
		{
			// Add to existing cart
			var cartDetail = new Cartdetail
			{
				CartId = existingCart.CartId,
				GameId = gameId,
				Quantity = quantity
			};

			existingCart.Cartdetails.Add(cartDetail);
			await _cartRepository.Update(existingCart);
			return existingCart;
		}
	}

}