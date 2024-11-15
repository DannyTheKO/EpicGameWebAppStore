using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartController : Controller
{
	private readonly ICartService _cartService;
	private readonly IGameService _gameService;
	private readonly IAccountService _accountService;
	private readonly IPaymentMethodService _paymentMethodService;

	public CartController(
		ICartService cartService,
		IGameService gameService,
		IAccountService accountService,
		IPaymentMethodService paymentMethodService)
	{
		_cartService = cartService;
		_gameService = gameService;
		_accountService = accountService;
		_paymentMethodService = paymentMethodService;
	}

	// GET: api/Cart
	[HttpGet("GetAll")]
	public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
	{
		var carts = await _cartService.GetAllCarts();
		return Ok(carts);
	}

	// GET: api/Cart/5
	[HttpGet("GetCart")]
	public async Task<ActionResult<Cart>> GetCartById([FromQuery] int cartId)
	{
		var cart = await _cartService.GetCartById(cartId);
		if (cart == null) return NotFound();
		return Ok(cart);
	}


	[HttpPost("CreateCart")]
	public async Task<ActionResult<CartFormModel>> CreateCart([FromBody] CartFormModel cartFormModel)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
			});
		}

		// Check if that AccountId exist
		if (cartFormModel.AccountId == null)
		{
			return Ok(new { success = false, message = "Account do not exist!" });
		}

		// Check if that PaymentMethod exist
		if (cartFormModel.PaymentMethodId == null)
		{
			return Ok(new { success = false, message = "Payment do not exist!" });
		}

		// Create a new cart first
		var cart = new Cart
		{
			AccountId = cartFormModel.AccountId,
			PaymentMethodId = cartFormModel.PaymentMethodId,
			CreatedOn = DateTime.UtcNow,
			TotalAmount = 0
		};

		await _cartService.AddCart(cart);
		return Ok(new { success = true, message = "Cart created successfully" });
	}



	// PUT: Cart/UpdateCart/{id}
	[HttpPut("UpdateCart/{id}")]
	public async Task<ActionResult<Cart>> UpdateCart([FromQuery] int id, [FromBody] Cart cart)
	{
		if (id != cart.CartId) return NotFound(new
		{
			sucess = false,
			message = "Requested Cart do not existed!"
		});

		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				success = false,
				errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
			});
		}

		await _cartService.UpdateCart(cart);
		return Ok(new
		{
			sucess = true,
			message = "Updated cart successfully"
		});
	}

	// DELETE: Cart/DeleteCart/{id}
	[HttpDelete("DeleteCart/{id}")]
	public async Task<ActionResult> DeleteCart([FromBody] int id)
	{
		var cart = await _cartService.GetCartById(id);

		if (cart == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Cart do not existed or already deleted"
			});

		}

		return Ok(new
		{
			success = true,
			message = "Successfully Deleted Cart"
		});
	}

	// GET: Cart/Account/5
	[HttpGet("GetCartsByAccountId/{accountId}")]
	public async Task<ActionResult<IEnumerable<Cart>>> GetCartsByAccountId(int accountId)
	{
		var cart = _cartService.GetCartsByAccountId(accountId);

		if (cart == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Cart Empty"
			});
		}

		return Ok(new
		{
			success = true,
			message = "Cart Found",
			cartList = cart
		});
	}
}