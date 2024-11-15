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
	private readonly ICartdetailService _cartdetailService;
	private readonly IGameService _gameService;
	private readonly IAccountService _accountService;
	private readonly IPaymentMethodService _paymentMethodService;

	public CartController(
		ICartService cartService,
		ICartdetailService cartdetailService,
		IGameService gameService,
		IAccountService accountService,
		IPaymentMethodService paymentMethodService)
	{
		_cartService = cartService;
		_cartdetailService = cartdetailService;
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
			TotalAmount = 0,
		};

		await _cartService.AddCart(cart);
		return Ok(new { success = true, message = "Cart created successfully" });
	}



	// PUT: Cart/UpdateCart/{id}
	[HttpPut("UpdateCart/{cartId}")]
	public async Task<ActionResult<CartFormModel>> UpdateCart(int cartId, [FromBody] CartFormModel cartFormModel)
	{
		// Check if CartId exist
		var checkCartId = await _cartService.GetCartById(cartId);
		if (checkCartId == null) return NotFound(new
		{
			sucess = false,
			message = "Requested Cart do not exist!"
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

		// Check if AccountId exist
		var checkAccountId = await _accountService.GetAccountById(cartFormModel.AccountId);
		if (checkAccountId == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Requested Account do not exist!"
			});
		}

		// Check if PaymentMethodId exist
		var checkPaymentMethodId = await _paymentMethodService.GetPaymentMethodById(cartFormModel.PaymentMethodId);
		if (checkPaymentMethodId == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Requested Payment Method do not exist!"
			});
		}

		// Create a new cart
		var cart = new Cart()
		{
			CartId = cartId,
			AccountId = cartFormModel.AccountId,
			PaymentMethodId = cartFormModel.PaymentMethodId,
			TotalAmount = await _cartService.CalculateTotalAmount(cartId),
			CreatedOn = DateTime.UtcNow,
			Cartdetails = (await _cartdetailService.GetAllCartDetailByCartId(cartId)).ToList()
		};

		// Update Cart
		await _cartService.UpdateCart(cart);
		
		return Ok(new
		{
			sucess = true,
			message = "Updated cart successfully",
			data = cart
		});
	}

	// DELETE: Cart/DeleteCart/{id}
	[HttpDelete("DeleteCart/{cartId}")]
	public async Task<ActionResult> DeleteCart(int cartId)
	{
		var existingCart = await _cartService.GetCartById(cartId);

		if (existingCart == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Cart do not existed or already deleted"
			});

		}

		await _cartService.DeleteCart(cartId);

		return Ok(new
		{
			success = true,
			message = "Successfully Deleted Cart",
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