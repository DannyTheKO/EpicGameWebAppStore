using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartController : Controller
{
	private readonly IAccountService _accountService;
	private readonly ICartService _cartService;
	private readonly IPaymentMethodService _paymentMethodService;

	public CartController(
		ICartService cartService,
		IAccountService accountService,
		IPaymentMethodService paymentMethodService)
	{
		_cartService = cartService;
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
	[HttpGet("GetCart/{cartId}")]
	public async Task<ActionResult<Cart>> GetCartById([FromBody] int cartId)
	{
		var cart = await _cartService.GetCartById(cartId);
		if (cart == null) return NotFound();
		return Ok(cart);
	}

	// POST: Cart/CreateCart
	[HttpPost("CreateCart")]
	public async Task<ActionResult> CreateCart([FromBody] Cart cart)
	{
		//bool hasPermission = await _authorizationServices.UserHasPermission(GetCurrentLoginAccountId(), "create");

		//if (!hasPermission)
		//{
		//	return StatusCode(403, new { 
		//		success = false,
		//		message = "Access Denied: Insufficient permissions to create cart" 
		//	});
		//}

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

		await _cartService.AddCart(cart);
		return CreatedAtAction(
			nameof(GetCartById),
			new { id = cart.CartId },
			new
			{
				success = true,
				message = "Cart created successfully",
				cartList = cart
			}
		);
	}

	// PUT: Cart/UpdateCart/{id}
	[HttpPut("UpdateCart/{id}")]
	public async Task<ActionResult> UpdateCart([FromQuery] int id, [FromBody] Cart cart)
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