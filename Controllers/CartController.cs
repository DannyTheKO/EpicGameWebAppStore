using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
		var carts = await _cartService.GetAllCartsAsync();
		return Ok(carts);
	}

	// GET: api/Cart/5
	[HttpGet("GetCart/{id}")]
	public async Task<ActionResult<Cart>> GetCartById(int id)
	{
		var cart = await _cartService.GetCartByIdAsync(id);
		if (cart == null) return NotFound();
		return Ok(cart);
	}

	// POST: Cart/CreateCart
	[HttpPost("CreateCart")]
	public async Task<ActionResult<Cart>> CreateCart(Cart cart)
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

		await _cartService.AddCartAsync(cart);

		return CreatedAtAction(
			nameof(GetCartById),
			new { id = cart.CartId },
			new
			{
				success = true,
				message = "Cart created successfully",
				data = cart
			}
		);
	}

	// PUT: Cart/UpdateCart/{id}
	[HttpPut("UpdateCart/{id}")]
	public async Task<ActionResult<Cart>> UpdateCart(int id, Cart cart)
	{
		if (id != cart.CartId) return BadRequest();
		if (id != cart.CartId) return BadRequest();

		if (ModelState.IsValid)
		{
			await _cartService.UpdateCartAsync(cart);
			return NoContent();
		}
		return BadRequest(ModelState);
	}

	// DELETE: Cart/DeleteCart/{id}
	[HttpDelete("DeleteCart/{id}")]
	public async Task<ActionResult> DeleteCart(int id)
	{
		await _cartService.DeleteCartAsync(id);
		return NoContent();
	}

	// GET: Cart/Account/5
	[HttpGet("Account/{accountId}")]
	public async Task<IActionResult> GetCartsByAccountId(int accountId)
	{
		var carts = await _cartService.GetCartsByAccountIdAsync(accountId);
		if (carts == null) return NotFound();
		return Ok(carts);
	}
}