using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
	private readonly IDiscountService _discountService;
	private readonly IGameService _gameServices;

	public DiscountController(IDiscountService discountService, IGameService gameServices)
	{
		_discountService = discountService;
		_gameServices = gameServices;
	}

	// Get all discounts
	[HttpGet("GetAll")]
	public async Task<IActionResult> GetAllDiscounts()
	{
		var discounts = await _discountService.GetAllDiscountAsync();
		return Ok(discounts);
	}

	// Get discount by ID
	[HttpGet("{id}")]
	public async Task<IActionResult> GetDiscountById(int id)
	{
		var discount = await _discountService.GetDiscountByIdAsync(id);
		if (discount == null)
		{
			return NotFound("Discount not found");
		}
		return Ok(discount);
	}

	// Get discount by code
	[HttpGet("code/{code}")]
	public async Task<IActionResult> GetDiscountByCode(string code)
	{
		var discounts = await _discountService.GetDiscountByCode(code);
		return Ok(discounts);
	}

	// Get discount by Game ID
	[HttpGet("game/{gameId}")]
	public async Task<IActionResult> GetDiscountByGameId(int gameId)
	{
		var discounts = await _discountService.GetDiscountByGameId(gameId);
		return Ok(discounts);
	}

    // Add a new discount
    [HttpPost("addDiscount")]
    public async Task<IActionResult> AddDiscount([FromBody] Discount discount)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

		var newDiscount = await _discountService.AddDiscountAsync(discount);
		return CreatedAtAction(nameof(GetDiscountById), new { id = newDiscount.DiscountId }, newDiscount);
	}

	// Update an existing discount
	[HttpPut("UpdateDiscount/{id}")]
	public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountFormModel discountFormModel)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		var game = await _gameServices.GetGameById(discountFormModel.GameID);
		if (game == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Publisher ID not found"
			});
		}
		var discount = new Discount
		{
			DiscountId = id,
			GameId = discountFormModel.GameID,
			Percent = discountFormModel.Percent,
			Code = discountFormModel.Code,
			StartOn = discountFormModel.StartOn,
			EndOn = discountFormModel.EndOn,
			Game = game
		};

		try
		{
			var updatedDiscount = await _discountService.UpdateDiscountAsync(discount);
			return Ok(updatedDiscount);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}

	// Delete a discount
	[HttpDelete("DeleteDiscount/{id}")]
	public async Task<IActionResult> DeleteDiscount(int id)
	{
		try
		{
			var deletedDiscount = await _discountService.DeleteDiscountAsync(id);
			return Ok(deletedDiscount);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}
}
