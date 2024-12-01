using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.EndUser;

[Authorize]
[ApiController]
[Route("Store/[controller]")]
public class CheckoutPage : _BaseController
{
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;
	private readonly IAuthorizationServices _authorizationServices;

	public CheckoutPage(ICartService cartService,	
		ICartdetailService cartdetailService, 
		IAuthorizationServices authorizationServices) 
		: base(authorizationServices)
	{
		_cartService = cartService;
		_cartdetailService = cartdetailService;
	}

	[HttpGet("CurrentCartList")]
	public async Task<ActionResult<Cart>> GetCurrentCartList()
	{
		var carts = await _cartService.GetActiveCartByAccountId(GetCurrentDetailAccount().AccountId);
		return Ok(carts);
	}
}
