using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EpicGameWebAppStore.Controllers.EndUser;

[ApiController]
[Route("Store/[controller]")]
public class GamePage : _BaseController
{
	private readonly IAuthorizationServices _authorizationServices;
	private readonly IGameService _gameService;
	private readonly ICartService _cartService;

	public GamePage(
		IGameService gameService,
		ICartService cartService,
		IAuthorizationServices authorizationServices) : base(authorizationServices)
	{
		_gameService = gameService;
		_cartService = cartService;
	}


	[HttpGet("{gameId}")]
	public async Task<ActionResult<Game>> GetGameById(int gameId)
	{
		var game = await _gameService.GetGameById(gameId);
		if (game == null)
			return NotFound(new
			{
				success = false,
				message = "Requested game is not found!"
			});

		return Ok(game);
	}


	[Authorize]
	[HttpPost("AddToCart")]
	public async Task<ActionResult> AddToCart(int gameId)
	{
		var game = await _gameService.GetGameById(gameId);
		if (game == null)
			return NotFound(new
			{
				success = false,
				message = "Requested game is not found!"
			});


		await _cartService.AddGameToCart(GetCurrentDetailAccount().AccountId, gameId);
		var (lastestAccountCart, message)= await _cartService.GetLatestCart(GetCurrentDetailAccount().AccountId);
		return Ok(new
		{
			success = true,
			message = "Game has been add to your cart !",
            cartId = lastestAccountCart.CartId,
			cartItems = lastestAccountCart,
		});
	}
}
