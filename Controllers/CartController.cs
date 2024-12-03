 using Application.Interfaces;
using Domain.Entities;
using EpicGameWebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers;

[Route("[controller]")]
[ApiController]
public class CartController : _BaseController
{
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;
	private readonly IGameService _gameService;
	private readonly IAccountService _accountService;
	private readonly IPaymentMethodService _paymentMethodService;
	private readonly IAuthorizationServices _authorizationServices;

	public CartController(
		ICartService cartService,
		ICartdetailService cartdetailService,
		IGameService gameService,
		IAccountService accountService,
		IPaymentMethodService paymentMethodService,
		IAuthorizationServices authorizationServices) : base(authorizationServices)
	{
		_cartService = cartService;
		_cartdetailService = cartdetailService;
		_gameService = gameService;
		_accountService = accountService;
		_paymentMethodService = paymentMethodService;
	}

	// GET: Cart/GetAll
	[HttpGet("GetAll")]
	public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
	{
		var carts = await _cartService.GetAllCarts();

        var formattedResponse = carts.GroupBy(c => c.AccountId)
            .Select(group => new
            {
                accountId = group.Key,
                name = group.First().Account.Username,
                cart = group.Select(c => new
                {
                    cartId = c.CartId,
					cartStatus = c.CartStatus,
					paymentMethodId = c.PaymentMethodId,
					paymentMethod = c.PaymentMethod.Name,
					cartDetails = c.Cartdetails.Select(cd => new
                    {
                        cartDetailId = cd.CartDetailId,
                        cartDetail = new
                        {
                            gameId = cd.Game.GameId,
                            title = cd.Game.Title,
                            price = cd.Price,
                            discount = cd.Discount
                        }
                    }).ToList()
                }).ToList()
            });

        return Ok(formattedResponse);
    }

	// GET: Cart/GetCartId
	[HttpGet("GetCartId/{cartId}")]
	public async Task<ActionResult<Cart>> GetCartById(int cartId)
	{
		var cart = await _cartService.GetCartById(cartId);
		if (cart == null) return NotFound();
		return Ok(cart);
	}

	// GET: Cart/GetCartsByAccountId/{accountId}
	[HttpGet("GetCartsByAccountId/{accountId}")]
	public async Task<ActionResult<IEnumerable<Cart>>> GetCartsByAccountId(int accountId)
	{
		var cart = await _cartService.GetCartsByAccountId(accountId);

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
			data = cart
		});
	}

	// GET: Cart/CheckOut/{accountId}
	[HttpGet("CheckOut/{accountId}")]
	public async Task<ActionResult<Cart>> GetLatestCart(int accountId)
	{
		var (cart, message)= await _cartService.GetLatestCart(accountId);
		if (cart == null)
		{
			return NotFound(new
			{
				success = false,
				message = message
			});
		}

		return Ok( new
		{
			success = true,
			message,
			data = cart
		});
	}



	// POST: Cart/CreateCart
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

	//POST: Cart/AddToCart
	[HttpPost("AddToCart")]
	public async Task<ActionResult> AddToCart(int gameId)
	{
		var checkGame = await _gameService.GetGameById(gameId);
		if (checkGame == null) // Not existed
		{
			return BadRequest(new
			{
				success = false,
				message = "Game do not exist"
			});
		}

		var (cart,message) = await _cartService.AddGameToCart(GetCurrentDetailAccount().AccountId, gameId);
		return Ok(new
		{
			success = true,
			message = message,
			data = cart,
		});
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


}