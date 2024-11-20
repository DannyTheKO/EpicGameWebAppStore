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

	public async Task AddCart(Cart cart)
	{
		// Retrieve the existing cart from the database
		var existingCart = await _cartRepository.GetById(cart.CartId);
		if (existingCart != null) throw new Exception("Cart already exists.");

		// Add the new cart to the database
		await _cartRepository.Add(cart);
	}

	public async Task UpdateCart(Cart cart)
	{
		// Retrieve the existing cart from the database
		var existingCart = await _cartRepository.GetById(cart.CartId);
		if (existingCart == null) throw new Exception("Cart not found.");

		// Update the cart details
		existingCart.AccountId = cart.AccountId;
		existingCart.PaymentMethodId = cart.PaymentMethodId;

		await _cartRepository.Update(existingCart);
	}

	public async Task DeleteCart(int id)
	{
		// Retrieve the existing cart from the database
		var existingCart = await _cartRepository.GetById(id);
		if (existingCart == null) throw new Exception("Cart not found.");

		await _cartRepository.Delete(id);
	}

	public async Task<Cart> GetCartById(int id)
	{
		return await _cartRepository.GetById(id);
	}
	#endregion

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





	public async Task<Cart> AddToCart(int accountId, int gameId)
	{
		var existingCarts = await _cartRepository.GetAllCartFromAccountId(accountId);
		var existingCart = existingCarts.FirstOrDefault(); // Get most recent cart or create new one

		if (existingCart == null) // Cart doesn't exist for that account
		{
			// Create new cart
			var newCart = new Cart()
			{
				AccountId = accountId,
				CreatedOn = DateTime.UtcNow,
				TotalAmount = 0,
				Cartdetails = new List<Cartdetail>()
			};

			// Add new cart detail
			var cartDetail = new Cartdetail
			{
				GameId = gameId,
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
			};

			existingCart.Cartdetails.Add(cartDetail);

			existingCart.TotalAmount = await CalculateTotalAmount(existingCart.CartId);
			await _cartRepository.Update(existingCart);
			return existingCart;
		}
	}

	// ACTION: Calculate Total Amount of all CartDetail with Discount and Price
	public async Task<decimal> CalculateTotalAmount(int cartId)
	{
		var cart = await _cartRepository.GetById(cartId);
		if (cart == null) throw new Exception("Cart not found.");

		decimal totalAmount = cart.Cartdetails.Sum(cd => 
			cd.Price.GetValueOrDefault() * 
			(1 - cd.Discount.GetValueOrDefault() / 100)
		);

		return totalAmount;
	}
}