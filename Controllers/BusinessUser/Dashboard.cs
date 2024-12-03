using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using EpicGameWebAppStore.Models;
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
	private readonly IRoleService _roleService;

	private readonly IGameService _gameService;
	private readonly IImageGameService _imageGameService;
	private readonly IPublisherService _publisherService;
	private readonly IGenreService _genreService;

	private readonly IDiscountService _discountService;
	private readonly IPaymentMethodService _paymentMethodService;
	private readonly ICartService _cartService;
	private readonly ICartdetailService _cartdetailService;

	private readonly IAuthorizationServices _authorizationServices;

	public Dashboard(
		IAccountService accountService,
		IAccountGameService accountGameService,
		IRoleService roleService,

		IGameService gameService,
		IImageGameService imageGameService,
		IPublisherService publisherService,
		IGenreService genreService,

		IDiscountService discountService,
		ICartService cartService,
		ICartdetailService cartdetailService,
		IPaymentMethodService paymentMethodService,

		IAuthorizationServices authorizationServices
		)
		: base(authorizationServices)
	{
		_accountService = accountService;
		_accountGameService = accountGameService;
		_roleService = roleService;

		_gameService = gameService;
		_imageGameService = imageGameService;
		_publisherService = publisherService;
		_genreService = genreService;

		_discountService = discountService;
		_cartService = cartService;
		_cartdetailService = cartdetailService;
		_paymentMethodService = paymentMethodService;

		_authorizationServices = authorizationServices;
	}


	#region == Game Tab ==

	// GET: Get all games
	[HttpGet("Game/GetAll")]
	public async Task<ActionResult<IEnumerable<Game>>> GetAll()
	{
		var games = await _gameService.GetAllGame();
		return Ok(games);
	}

	// GET: Get game by ID
	[HttpGet("Game/GetById/{gameId}")]
	public async Task<ActionResult<Game>> GetGameById(int gameId)
	{
		var game = await _gameService.GetGameById(gameId);
		if (game == null) return NotFound(new
		{
			success = false,
			message = "Requested game is not found!"
		});

		return Ok(game);
	}

	// POST: Create game
	[HttpPost("Game/Create")]
	public async Task<ActionResult> CreateGame([FromForm] GameFormModel gameFormModel, IFormFile imageFile)
	{
		await CheckPermission("add");

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

		// Check if Publisher ID is available
		var checkPublisher = await _publisherService.GetPublisherById(gameFormModel.PublisherId);
		if (checkPublisher == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Publisher ID not found"
			});
		}

		// Check if Genre ID is available
		var checkGenre = await _genreService.GetGenreById(gameFormModel.GenreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre ID not found"
			});
		}

		// Check if Game name is already existed
		var checkGame = await _gameService.GetGameByTitle(gameFormModel.Title);
		var existingGame = checkGame.FirstOrDefault();

		if (existingGame != null)
		{
			// If game exists and image is provided, add it to existing game
			if (imageFile != null)
			{
				var (imageGame, flag) = await _imageGameService.UploadImageGame(imageFile, existingGame.GameId, gameFormModel.ImageType);
				if (!flag)
				{
					return BadRequest(new
					{
						success = flag,
						message = "Failed to upload image",
					});
				}

				return Ok(new
				{
					success = true,
					message = "Image added to existing game successfully",
					data = new
					{
						game = existingGame,
						newImage = imageGame
					}
				});
			}

			return Ok(new
			{
				success = true,
				message = "Game already exists",
				data = existingGame
			});
		}

		// If game doesn't exist, create new game
		var game = new Game()
		{
			PublisherId = gameFormModel.PublisherId,
			GenreId = gameFormModel.GenreId,
			Title = gameFormModel.Title,
			Price = gameFormModel.Price,
			Author = gameFormModel.Author,
			Release = DateTime.UtcNow,
			Rating = gameFormModel.Rating,
			Description = gameFormModel.Description,
		};

		await _gameService.AddGame(game);

		if (imageFile != null)
		{
			var (imageGame, flag) = await _imageGameService.UploadImageGame(imageFile, game.GameId, gameFormModel.ImageType);
			if (!flag)
			{
				return BadRequest(new
				{
					success = flag,
					message = "Failed to upload image",
					data = imageGame
				});
			}
		}

		await _gameService.UpdateGame(game);

		return Ok(new
		{
			success = true,
			message = "Successfully added new game",
			data = game
		});
	}

	// PUT: Update game
	[HttpPut("Game/Update/{gameId}")]
	public async Task<ActionResult> UpdateGame([FromBody] GameFormModel gameFormModel, int gameId)
	{
		await CheckPermission("update");

		// Check if user input is valid
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

		// Check if the game ID is existed in the database
		var checkGame = await _gameService.GetGameById(gameId);
		if (checkGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game ID not found"
			});
		}

		// Check if the publisher ID is existed in the database
		var checkPublisher = await _publisherService.GetPublisherById(gameFormModel.PublisherId);
		if (checkPublisher == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Publisher ID not found"
			});
		}

		// Check if the genre ID is existed in the database
		var checkGenre = await _genreService.GetGenreById(gameFormModel.GenreId);
		if (checkGenre == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Genre ID not found"
			});
		}

		// Create new game
		var game = new Game()
		{
			GameId = checkGame.GameId,
			PublisherId = gameFormModel.PublisherId,
			GenreId = gameFormModel.GenreId,
			Title = gameFormModel.Title,
			Price = gameFormModel.Price,
			Author = gameFormModel.Author,
			Rating = gameFormModel.Rating,
			Description = gameFormModel.Description,
			Release = gameFormModel.Release,

			// Get detail from existing game
			Genre = checkGenre,
			Publisher = checkPublisher,
		};

		await _gameService.UpdateGame(game);
		return Ok(new
		{
			success = true,
			message = "Update Game Successfully",
			data = game
		});
	}

	// DELETE: Delete game
	[HttpDelete("Game/Delete/{gameId}")]
	public async Task<ActionResult> DeleteGame(int gameId)
	{
		await CheckPermission("delete");

		var existingGame = await _gameService.GetGameById(gameId);
		if (existingGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "ID game don't match with the database or the game is deleted"
			});
		}

		await _gameService.DeleteGame(gameId);
		return Ok(new
		{
			success = false,
			message = "Delete Game Success"
		});
	}

	#endregion

	#region == Account Game Tab ==

	// GET: Get all account game
	[HttpGet("AccountGame/GetAll")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAllAccountGame()
	{
		var accountGameList = await _accountGameService.GetAllAccountGame();
		return Ok(accountGameList);
	}

	// GET: Get account game by account game ID
	[HttpGet("AccountGame/GetById/{accountGameId}")]
	public async Task<ActionResult<AccountGame>> GetAccountGameById(int accountGameId)
	{
		var accountGame = await _accountGameService.GetAccountGameById(accountGameId);
		return Ok(accountGame);
	}

	// GET: Get account game by game ID
	[HttpGet("AccountGame/GetByGameId/{gameId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByGameId(int gameId)
	{
		// Retrieve Game by Game ID
		var accountGameList = await _accountGameService.GetAccountGameByGameId(gameId);
		if (accountGameList.FirstOrDefault() == null) // NOT FOUND
		{
			return BadRequest(new
			{
				success = false,
				message = "That Game doesn't associate with any Account."
			});
		}

		return Ok(new
		{
			success = true,
			message = "Found!",
			data = accountGameList
		});
	}

	// GET: Get account game by account ID
	[HttpGet("AccountGame/GetByAccountId/{accountId}")]
	public async Task<ActionResult<IEnumerable<AccountGame>>> GetAccountGameByAccountId(int accountId)
	{
		var accountGameList = await _accountGameService.GetAccountGameByAccountId(accountId);
		if (accountGameList == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account doesn't associate with any Game."
			});
		}

		return Ok(new
		{
			success = true,
			message = "Found!",
			data = accountGameList
		});
	}


	// POST: Add account games
	[HttpPost("AccountGame/Add")]
	public async Task<ActionResult<AccountGameFormModel>> AddAccountGame([FromBody] AccountGameFormModel accountGameFormModel)
	{
		await CheckPermission("add");

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

		// Retrieve both Account, Game and AccountGame data
		var checkAccount = await _accountService.GetAccountById(accountGameFormModel.AccountId);
		var checkGame = await _gameService.GetGameById(accountGameFormModel.GameId);

		// Check if both Account and game exist 
		if (checkAccount == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Account is not found!"
			});
		}

		if (checkGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game is not found!"
			});
		}

		// Check if that Account already has that game
		var checkAccountGame = (await _accountGameService.GetAccountGameByAccountId(accountGameFormModel.AccountId))
			.Where(g => g.GameId == checkGame.GameId);

		if (checkAccountGame.Any())
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account already has that Game."
			});
		}


		// Create new Account Game
		var accountGame = new AccountGame
		{
			AccountId = checkAccount.AccountId,
			GameId = checkGame.GameId,
			DateAdded = DateTime.UtcNow,
		};

		await _accountGameService.AddAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "Successfully added AccountGame",
			data = accountGame,
		});

	}

	// PUT: Update account game
	[HttpPut("AccountGame/Update/{accountGameId}")]
	public async Task<ActionResult> UpdateAccountGame(int accountGameId, [FromBody] AccountGameFormModel accountGameFormModel)
	{
		await CheckPermission("update");

		// Validate user input
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

		var checkAccountGame = await _accountGameService.GetAccountGameById(accountGameId);
		if (checkAccountGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "AccountGame not found"
			});
		}

		// Check if that Account already has that game (excluding the current accountGameId)
		var existingGames = (await _accountGameService.GetAccountGameByAccountId(accountGameFormModel.AccountId))
			.Where(g => g.GameId == accountGameFormModel.GameId && g.AccountGameId != accountGameId);

		if (existingGames.Any())
		{
			return BadRequest(new
			{
				success = false,
				message = "That Account already has that Game."
			});
		}

		// Rest of your existing update logic...
		var accountGame = new AccountGame
		{
			AccountGameId = accountGameId,
			AccountId = accountGameFormModel.AccountId,
			GameId = accountGameFormModel.GameId,
			DateAdded = DateTime.UtcNow,
		};

		await _accountGameService.UpdateAccountGame(accountGame);
		return Ok(new
		{
			success = true,
			message = "AccountGame updated successfully",
			data = accountGame
		});
	}

	// DELETE: delete account game
	[HttpDelete("AccountGame/Delete/{accountGameId}")]
	public async Task<ActionResult> DeleteAccountGame(int accountGameId)
	{
		await CheckPermission("delete");

		var accountGame = await _accountGameService.GetAccountGameById(accountGameId);
		if (accountGame == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "AccountGame do not exist or already deleted."
			});
		}

		await _accountGameService.DeleteAccountGame(accountGameId);
		return Ok(new
		{
			success = true,
			message = "AccountGame Successfully Deleted."
		});
	}

	#endregion

	#region == Account ==

	// GET: Get all account
	[HttpGet("Account/GetAll")]
	public async Task<ActionResult<IEnumerable<Account>>> GetAllAccount()
	{
		var accountList = await _accountService.GetAllAccounts();
		return Ok(accountList);
	}

	// GET: Get account by accountID
	[HttpGet("Account/GetById/{accountId}")]
	public async Task<ActionResult<Account>> GetAccountById(int accountId)
	{
		// Get Account by accountId
		var account = await _accountService.GetAccountById(accountId);

		// Check if that account is valid
		if (account == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Request Account is not available"
			});
		}

		return Ok(new
		{
			success = true,
			message = "Account Found",
			account,
		});
	}

	// POST: Add Account
	[HttpPost("Account/Add")]
	public async Task<ActionResult> AddAccount([FromBody] AccountFormModel accountFormModel)
	{
		await CheckPermission("add");

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

		// Check if username already exists
		var existingUsername = await _accountService.GetAccountByUsername(accountFormModel.Username);
		if (existingUsername != null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Username already exists"
			});
		}

		// Create new account entity
		var account = new Account
		{
			Username = accountFormModel.Username,
			Password = accountFormModel.Password,
			Email = accountFormModel.Email,
			IsActive = accountFormModel.IsActive,
			RoleId = accountFormModel.RoleId,
			CreatedOn = DateTime.UtcNow
		};

		await _accountService.AddAccount(account);
		return Ok(new
		{
			success = true,
			message = "Account successfully added!",
			data = account
		});
	}

	// PUT: Update account by account ID
	[HttpPut("Account/Update/{accountId}")]
	public async Task<ActionResult> UpdateAccount(int accountId, [FromBody] AccountFormModel accountFormModel)
	{
		await CheckPermission("update");

		// Get Account by accountId
		var checkAccount = await _accountService.GetAccountById(accountId);
		if (checkAccount == null) // if that account is empty
		{
			return BadRequest(new
			{
				sucess = false,
				message = "Requested Account do not existed!"
			});
		}

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

		// Check if username is already existed
		var checkUsername = await _accountService.GetAccountByUsername(accountFormModel.Username);
		if (checkUsername != null && checkUsername.AccountId != accountId)
		{
			return BadRequest(new
			{
				success = false,
				message = "Username is already existed"
			});
		}

		// Check if email is already existed
		var checkEmail = await _accountService.GetAccountByEmail(accountFormModel.Email);
		if (checkEmail != null && checkEmail.AccountId != accountId)
		{
			return BadRequest(new
			{
				success = false,
				message = "Email is already existed"
			});
		}

		// Check if that RoleId is exist
		var checkRole = await _roleService.GetRoleById(accountFormModel.RoleId);
		if (checkRole == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Role not found"
			});
		}

		// Create a new account
		var account = new Account
		{
			AccountId = accountId,
			Username = accountFormModel.Username,
			Password = accountFormModel.Password,
			Email = accountFormModel.Email,
			RoleId = accountFormModel.RoleId,
			Role = await _roleService.GetRoleById(accountFormModel.RoleId),
			IsActive = accountFormModel.IsActive,
			CreatedOn = DateTime.UtcNow
		};

		await _accountService.UpdateAccount(account);
		return Ok(new
		{
			success = true,
			message = "Successfully Updated Account",
			account
		});
	}

	// DELETE: Delete account base on accountID
	[HttpDelete("Account/Delete/{accountId}")]
	public async Task<ActionResult> DeleteAccount(int accountId)
	{
		await CheckPermission("delete");

		var account = await _accountService.GetAccountById(accountId);

		if (account == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Requested Account do not existed or already deleted"
			});
		}

		await _accountService.DeleteAccount(accountId);
		return Ok(new
		{
			success = true,
			message = "Successfully Deleted Account"
		});
	}

	#endregion

	#region == Discount ==

	// GET: Get all discounts
	[HttpGet("Discount/GetAll")]
	public async Task<ActionResult> GetAllDiscounts()
	{
		var discounts = await _discountService.GetAllDiscountAsync();
		return Ok(discounts);
	}

	// Get discount by ID
	[HttpGet("Discount/GetById/{discountId}")]
	public async Task<ActionResult> GetDiscountById(int discountId)
	{
		var discount = await _discountService.GetDiscountByIdAsync(discountId);
		if (discount == null)
		{
			return NotFound("Discount not found");
		}
		return Ok(discount);
	}

	// Get discount by code
	[HttpGet("Discount/GetByCode/{discountCode}")]
	public async Task<ActionResult> GetDiscountByCode(string code)
	{
		var discounts = await _discountService.GetDiscountByCode(code);
		return Ok(discounts);
	}

	// Get discount by Game ID
	[HttpGet("Discount/GetByGameId/{gameId}")]
	public async Task<ActionResult> GetDiscountByGameId(int gameId)
	{
		var discounts = await _discountService.GetDiscountByGameId(gameId);
		return Ok(discounts);
	}

	// Add a new discount
	[HttpPost("Discount/Add")]
	public async Task<IActionResult> AddDiscount([FromBody] DiscountFormModel discountFormModel)
	{
		await CheckPermission("add");

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

		// Check if Game exists
		var game = await _gameService.GetGameById(discountFormModel.GameID);
		if (game == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game not found"
			});
		}

		// Create new discount entity
		var discount = new Discount
		{
			GameId = discountFormModel.GameID,
			Percent = discountFormModel.Percent,
			Code = discountFormModel.Code,
			StartOn = discountFormModel.StartOn,
			EndOn = discountFormModel.EndOn,
			Game = game
		};

		var createdDiscount = await _discountService.AddDiscountAsync(discount);
		return Ok(new
		{
			success = true,
			message = "Discount created successfully",
			data = createdDiscount
		});
	}

	// Update an existing discount
	[HttpPut("Discount/Update/{discountId}")]
	public async Task<ActionResult> UpdateDiscount(int discountId, [FromBody] DiscountFormModel discountFormModel)
	{
		await CheckPermission("update");

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

		var game = await _gameService.GetGameById(discountFormModel.GameID);
		if (game == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game not found"
			});
		}

		var discount = new Discount
		{
			DiscountId = discountId,
			GameId = discountFormModel.GameID,
			Percent = discountFormModel.Percent,
			Code = discountFormModel.Code,
			StartOn = discountFormModel.StartOn,
			EndOn = discountFormModel.EndOn,
			Game = game
		};

		var updatedDiscount = await _discountService.UpdateDiscountAsync(discount);
		return Ok(new
		{
			success = true,
			message = "Discount updated successfully",
			data = updatedDiscount
		});
	}


	// Delete a discount
	[HttpDelete("DeleteDiscount/{discountId}")]
	public async Task<IActionResult> DeleteDiscount(int discountId)
	{
		await CheckPermission("delete");

		var discount = await _discountService.GetDiscountByIdAsync(discountId);
		if (discount == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Discount not found"
			});
		}

		var deletedDiscount = await _discountService.DeleteDiscountAsync(discountId);
		return Ok(new
		{
			success = true,
			message = "Discount deleted successfully",
			data = deletedDiscount
		});
	}


	#endregion

	#region == Publisher ==

	// Get all publishers
	[HttpGet("Publisher/GetAll")]
	public async Task<ActionResult> GetAllPublishers()
	{
		var publishers = await _publisherService.GetAllPublishers();
		return Ok(publishers);
	}

	// Get publisher by ID
	[HttpGet("Publisher/GetById/{publisherId}")]
	public async Task<ActionResult> GetPublisherById(int publisherId)
	{
		var publisher = await _publisherService.GetPublisherById(publisherId);
		if (publisher == null)
		{
			return NotFound("Publisher not found.");
		}
		return Ok(publisher);
	}

	// Get publishers by name
	[HttpGet("Publisher/GetByName/{publisherName}")]
	public async Task<ActionResult> GetPublishersByName(string publisherName)
	{
		var publishers = await _publisherService.GetPublisherByName(publisherName);
		return Ok(publishers);
	}

	// Get publishers by address
	[HttpGet("Publisher/GetByAddress/{publisherAddress}")]
	public async Task<ActionResult> GetPublishersByAddress(string publisherAddress)
	{
		var publishers = await _publisherService.GetPublisherByAddress(publisherAddress);
		return Ok(publishers);
	}

	// Add a new publisher
	[HttpPost("Publisher/Create")]
	public async Task<ActionResult> AddPublisher([FromBody] Publisher publisher)
	{
		await CheckPermission("add");

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var newPublisher = await _publisherService.AddPublisher(publisher);
		return CreatedAtAction(nameof(GetPublisherById), new { id = newPublisher.PublisherId }, newPublisher);
	}

	// Update an existing publisher
	[HttpPut("Publisher/Update/{publisherId}")]
	public async Task<ActionResult> UpdatePublisher(int publisherId, [FromBody] PublisherFormModel publisherFormModel)
	{
		await CheckPermission("update");

		if (!ModelState.IsValid) return BadRequest(ModelState);
		var publisher = new Publisher
		{
			PublisherId = publisherId,
			Name = publisherFormModel.Name,
			Address = publisherFormModel.Address,
			Email = publisherFormModel.Email,
			Phone = publisherFormModel.Phone,
			Website = publisherFormModel.Website,
		};
		try
		{
			var updatedPublisher = await _publisherService.UpdatePublisher(publisher);
			return Ok(updatedPublisher);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}

	// Delete a publisher
	[HttpDelete("Publisher/Delete/{publisherId}")]
	public async Task<ActionResult> DeletePublisher(int publisherId)
	{
		await CheckPermission("delete");

		try
		{
			var deletedPublisher = await _publisherService.DeletePublisher(publisherId);
			return Ok(deletedPublisher);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}

	#endregion

	#region == Cart ==

		// GET: Cart/GetAll
	[HttpGet("Cart/GetAll")]
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
	[HttpGet("Cart/GetById/{cartId}")]
	public async Task<ActionResult<Cart>> GetCartById(int cartId)
	{
		var cart = await _cartService.GetCartById(cartId);
		if (cart == null) return NotFound();
		return Ok(cart);
	}

	// GET: Cart/GetCartsByAccountId/{accountId}
	[HttpGet("Cart/GetByAccountId/{accountId}")]
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
	[HttpGet("Cart/CheckOut/{accountId}")]
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
	[HttpPost("Cart/CreateCart")]
	public async Task<ActionResult<CartFormModel>> CreateCart([FromBody] CartFormModel cartFormModel)
	{
		await CheckPermission("add");

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

	// PUT: Cart/UpdateCart/{id}
	[HttpPut("Cart/Update/{cartId}")]
	public async Task<ActionResult<CartFormModel>> UpdateCart(int cartId, [FromBody] CartFormModel cartFormModel)
	{
		await CheckPermission("update");

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
	[HttpDelete("Cart/Delete/{cartId}")]
	public async Task<ActionResult> DeleteCart(int cartId)
	{
		await CheckPermission("delete");

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


	#endregion

	#region == Image ==

	// GET: Get all Image
	[HttpGet("Image/GetAll")]
	public async Task<ActionResult<IEnumerable<ImageGame>>> GetAllImage()
	{
		var imageList = await _imageGameService.GetAllImageGame();
		return Ok(imageList);
	}

	// GET: Get Image by ID
	[HttpGet("Image/GetById/{imageId}")]
	public async Task<ActionResult> GetImageById(int imageId)
	{
		var image = await _imageGameService.GetImageGameById(imageId);
		if (image == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Image not found"
			});
		}
		return Ok(image);
	}

	// GET: Get Image by Game ID
	[HttpGet("Image/GetByGameId/{gameId}")]
	public async Task<ActionResult<IEnumerable<ImageGame>>> GetImageByGameId(int gameId)
	{
		var images = await _imageGameService.GetImageGameByGameId(gameId);
		return Ok(images);
	}

	[HttpPost("Image/Add")]
	public async Task<ActionResult> AddImage([FromForm] ImageFormModel imageFormModel)
	{
		await CheckPermission("add");

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
		// Check if Game exists
		var game = await _gameService.GetGameById(imageFormModel.GameId);
		if (game == null)
		{
			return BadRequest(new
			{
				success = false,
				message = "Game not found"
			});
		}
		// Create new image entity
		var image = new ImageGame
		{
			GameId = imageFormModel.GameId,
			ImageType = imageFormModel.ImageType,
			Game = game
		};
		var createdImage = await _imageGameService.AddImageGame(image);
		return Ok(new
		{
			success = true,
			message = "Image created successfully",
			data = createdImage
		});
	}


	// DELETE: Delete Image
	[HttpDelete("Image/Delete/{imageId}")]
	public async Task<ActionResult> DeleteImage(int imageId)
	{
		await CheckPermission("delete");

		var image = await _imageGameService.GetImageGameById(imageId);
		if (image == null)
		{
			return NotFound(new
			{
				success = false,
				message = "Image not found"
			});
		}
		await _imageGameService.DeleteImageGame(imageId);
		return Ok(new
		{
			success = true,
			message = "Image deleted successfully"
		});
	}

	#endregion

}
