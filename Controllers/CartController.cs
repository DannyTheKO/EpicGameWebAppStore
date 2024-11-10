using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartController : _BaseController
{
	private readonly IAccountService _accountService;
	private readonly IAuthenticationServices _authenticationServices;
	private readonly IAuthorizationServices _authorizationServices;
	private readonly ICartService _cartService;
	private readonly IPaymentMethodService _paymentMethodService;

	public CartController(
		ICartService cartService,
		IAccountService accountService,
		IRoleService roleService,
		IPaymentMethodService paymentMethodService,
		IAuthenticationServices authenticationServices,
		IAuthorizationServices authorizationServices)
		: base(
			authenticationServices,
			authorizationServices,
			accountService,
			roleService)
	{
		_cartService = cartService;
		_accountService = accountService;
		_paymentMethodService = paymentMethodService;
	}

	// GET: api/Cart
	[HttpGet("GetAllCarts")]
	public async Task<IActionResult> GetAllCarts()
	{
		var carts = await _cartService.GetAllCartsAsync();
		return Ok(carts);
	}

	// GET: api/Cart/5
	[HttpGet("{id}")]
	public async Task<IActionResult> GetCartById(int id)
	{
		var cart = await _cartService.GetCartByIdAsync(id);
		if (cart == null) return NotFound();
		return Ok(cart);
	}

	// POST: api/Cart
	[HttpPost]
	public async Task<IActionResult> CreateCart([FromBody] Cart cart)
	{
		if (ModelState.IsValid)
		{
			await _cartService.AddCartAsync(cart);
			return CreatedAtAction(nameof(GetCartById), new { id = cart.CartId }, cart);
		}
        }
		return BadRequest(ModelState);
	}
    }
	// PUT: api/Cart/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCart(int id, [FromBody] Cart cart)
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
    }
	// DELETE: api/Cart/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCart(int id)
	{
		await _cartService.DeleteCartAsync(id);
		return NoContent();
	}
        return RedirectToAction(nameof(Index));
	// GET: api/Cart/Account/5
	[HttpGet("Account/{accountId}")]
	public async Task<IActionResult> GetCartsByAccountId(int accountId)
	{
		var carts = await _cartService.GetCartsByAccountIdAsync(accountId);
		if (carts == null) return NotFound();
		return Ok(carts);
	}
}
    }
}