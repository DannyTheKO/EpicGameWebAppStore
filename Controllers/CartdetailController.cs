using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartdetailController : Controller
{
	private readonly ICartdetailService _cartdetailService;
	private readonly ICartService _cartService;
	private readonly IGameService _gameService;

	public CartdetailController(ICartdetailService cartdetailService, IGameService gameService, ICartService cartService)
	{
		_cartdetailService = cartdetailService;
		_gameService = gameService;
		_cartService = cartService;
	}

	[HttpGet("GetAllCartDetail")]
	public async Task<ActionResult<IEnumerable<Cartdetail>>> GetAllCartDetail()
	{
		var cartDetail = await _cartdetailService.GetAllCartDetails();
		return Ok(cartDetail);
	}

	[HttpGet("GetCartDetail/{id}")]
	public async Task<ActionResult<Cartdetail>> GetCartDetail(int id)
	{
		var cartDetail = await _cartdetailService.GetCartDetailById(id);
		if (cartDetail == null)
			return NotFound(new { success = false, message = "Cart detail not found" });
		return Ok(cartDetail);
	}

	[HttpPost("AddCartDetail")]
	public async Task<ActionResult<CartDetailFormModel>> AddCartDetail([FromBody] CartDetailFormModel cartDetailFormModel)
	{
		// Check if the user input is valid
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

		// Check if that CartId is exist
		var checkCart = await _cartService.GetCartById(cartDetailFormModel.CartId);
		if (checkCart == null)
		{
			return NotFound(new { success = false, message = "Cart not found" });
		}

		// Check if that GameId is exist
		var checkGame = await _gameService.GetGameById(cartDetailFormModel.GameId);
		if (checkGame == null)
		{
			return NotFound(new { success = false, message = "Game not found" });
		}

		// Create a new cart detail
		var cartDetail = new Cartdetail()
		{
			CartId = cartDetailFormModel.CartId,
			GameId = cartDetailFormModel.GameId,
			Quantity = cartDetailFormModel.Quantity,
			Price = (await _gameService.GetGameById(cartDetailFormModel.GameId)).Price,
			Discount = cartDetailFormModel.Discount
		};

		// Add the cart detail to the database
		await _cartdetailService.AddCartDetail(cartDetail);

		// Return the result
		return Ok(new
		{
			success = true,
			message = "Cart detail added successfully",
			data = cartDetail
		});
	}

	[HttpPut("UpdateCartDetail")]
	public async Task<ActionResult<CartDetailFormModel>> UpdateCartDetail([FromBody] CartDetailFormModel cartDetailFormModel)
	{
		// Check if the user input is valid
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

		// Check if that CartId is exist
		var checkCart = await _cartService.GetCartById(cartDetailFormModel.CartId);
		if (checkCart == null)
		{
			return NotFound(new { success = false, message = "Cart not found" });
		}

		// Check if that GameId is exist
		var checkGame = await _gameService.GetGameById(cartDetailFormModel.GameId);
		if (checkGame == null)
		{
			return NotFound(new { success = false, message = "Game not found" });
		}

		// Create a new cart detail
		var cartDetail = new Cartdetail()
		{
			CartId = cartDetailFormModel.CartId,
			GameId = cartDetailFormModel.GameId,
			Quantity = cartDetailFormModel.Quantity,
			Price = (await _gameService.GetGameById(cartDetailFormModel.GameId)).Price,
			Discount = cartDetailFormModel.Discount
		};

		// Update the cart detail to the database
		await _cartdetailService.UpdateCartDetail(cartDetail);

		// Return the result
		return Ok(new
		{
			success = true,
			message = "Cart detail updated successfully",
			data = cartDetail
		});
	}

	[HttpDelete("DeleteCartDetail/{id}")]
	public async Task<ActionResult<Cartdetail>> DeleteCartDetail(int id)
	{
		var result = await _cartdetailService.DeleteCartDetail(id);
		return Ok(new { success = true, message = "Cart detail deleted successfully", data = result });
	}
}