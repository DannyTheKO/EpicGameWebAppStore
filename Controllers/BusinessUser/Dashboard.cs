using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers.BusinessUser;


[ApiController]
[Route("Store/[controller]")]
[Authorize(Roles = "Admin, Moderator")]
public class Dashboard : _BaseController
{
	private readonly IAccountService _accountService;
	private readonly IAccountGameService _accountGameService;
	private readonly IGameService _gameService;
	private readonly IDiscountService _discountService;
	private readonly IAuthorizationServices _authorizationServices;
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;

	public Dashboard(
		IAccountService accountService,
		IAccountGameService accountGameService,
		IGameService gameService,
		IDiscountService discount,
		IAuthorizationServices authorizationServices,
		ICartService cartService,
		ICartdetailService cartdetailService)
		: base(authorizationServices)
	{
		_accountService = accountService;
		_accountGameService = accountGameService;
		_gameService = gameService;
		_discountService = discount;
		_authorizationServices = authorizationServices;
		_cartService = cartService;
		_cartdetailService = cartdetailService;
	}

}
