using Application.Interfaces;
using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services;

public class CartService : ICartService
{
	private readonly ICartRepository _cartRepository;
	private readonly IGameRepository _gameRepository;
	private readonly IDiscountService _discountService;


	public CartService(ICartRepository cartRepository, IGameRepository gameRepository, IDiscountService discountService)
	{
		_cartRepository = cartRepository;
		_gameRepository = gameRepository;
		_discountService = discountService;
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

	public async Task<IEnumerable<Cart>> GetCompleteCartByAccountId(int accountId)
	{
		var cart = await _cartRepository.GetAllCartFromAccountId(accountId);
		var filtered = cart.Where(c => c.CartStatus == "Completed");

		return filtered;
	}

	public async Task<Cart> GetActiveCartByAccountId(int accountId)
	{
		var cart = await _cartRepository.GetAllCartFromAccountId(accountId);
		var filtered = cart.FirstOrDefault(c => c.CartStatus == "Active");
		return filtered;
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

	public async Task<(Cart, string Message)> GetLatestCart(int accountId)
	{
		var existingCarts = await _cartRepository.GetAllCartFromAccountId(accountId);
		var filtered = existingCarts.OrderByDescending(c => c.CreatedOn).FirstOrDefault();
		if (filtered == null)
		{
			return (null, "Cart Not Found!");
		}

		return (filtered, "Found!");
	}

	public async Task<(Cart, string Message)> AddGameToCart(int accountId, int gameId)
	{
		var existingCart = await GetActiveCartByAccountId(accountId);

		if (existingCart == null)
		{
			// Create new cart
			var newCart = new Cart
			{
				AccountId = accountId,
				PaymentMethodId = 1,
				CreatedOn = DateTime.UtcNow,
				CartStatus = "Active",
				TotalAmount = 0,
				Cartdetails = new List<Cartdetail>()
			};

			// Get game price and check for active discounts
			var game = await _gameRepository.GetById(gameId);
			var activeDiscounts = await _discountService.GetDiscountByGameId(gameId);
			var currentDiscount = activeDiscounts
				.Where(d => d.StartOn <= DateTime.UtcNow && d.EndOn >= DateTime.UtcNow)
				.OrderByDescending(d => d.Percent)
				.FirstOrDefault();

			// Create cart detail with discount if available
			var cartDetail = new Cartdetail
			{
				GameId = gameId,
				Price = game.Price,
				Discount = currentDiscount?.Percent ?? 0
			};

			newCart.Cartdetails.Add(cartDetail);
			await _cartRepository.Add(newCart);
			return (newCart, "Cart not found!, created a new cart with a new cartDetail");
		}
		else
		{
			// Check if game already exists in cart
			var gameExists = existingCart.Cartdetails.Any(cd => cd.GameId == gameId);
			if (gameExists)
			{
				return (existingCart, "This game is already in your cart!");
			}

			// Get active discount for the game
			var activeDiscounts = await _discountService.GetDiscountByGameId(gameId);
			var currentDiscount = activeDiscounts
				.Where(d => d.StartOn <= DateTime.UtcNow && d.EndOn >= DateTime.UtcNow)
				.OrderByDescending(d => d.Percent)
				.FirstOrDefault();

			// Add to existing cart with discount
			var cartDetail = new Cartdetail
			{
				CartId = existingCart.CartId,
				GameId = gameId,
				Price = (await _gameRepository.GetById(gameId)).Price,
				Discount = currentDiscount?.Percent ?? 0
			};

			existingCart.Cartdetails.Add(cartDetail);
			existingCart.TotalAmount = await CalculateTotalAmount(existingCart.CartId);
			await _cartRepository.Update(existingCart);
			return (existingCart, "Cart found, add cartDetail into the existed cart!");
		}
	}


	// ACTION: Complete Checkout Cart
	public async Task CompleteCheckout(int cartId, int paymentMethodId)
	{
		var cart = await _cartRepository.GetById(cartId);
		if (cart == null) throw new Exception("Cart not found.");

		cart.PaymentMethodId = paymentMethodId;
		cart.CartStatus = "Completed";
		await _cartRepository.Update(cart);
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