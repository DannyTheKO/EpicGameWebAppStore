using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartdetailController : Controller
{
	private readonly ICartdetailService _cartdetailService;

	public CartdetailController(ICartdetailService cartdetailService)
	{
		_cartdetailService = cartdetailService;
	}

	[HttpGet("GetAllCartDetail")]
	public async Task<ActionResult<IEnumerable<Cartdetail>>> GetAllCartDetail()
	{
		var cartDetail = await _cartdetailService.GetAllCartDetails();
		return Ok(cartDetail);
	}

	//public async Task<ActionResult> AddCartDetail(Cartdetail cartdetail)
	//{
		
	//}
}