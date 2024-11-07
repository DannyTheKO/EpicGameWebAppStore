using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EpicGameWebAppStore.Controllers
{
// Location: Controllers/_BaseController.cs
	public abstract class _BaseController : Controller
	{
		private readonly IAuthorizationServices _authorizationServices;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

		protected _BaseController(
			IAuthenticationServices authenticationServices,
			IAuthorizationServices authorizationServices,
            IAccountService accountService,
            IRoleService roleService)
		{
            _authenticationServices = authenticationServices;
			_authorizationServices = authorizationServices;
            _accountService = accountService;
            _roleService = roleService;
        }

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Set authentication status and username for all views
			ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;
			ViewData["Account_Username"] = User.Identity?.Name;

			// Add role information if user is authenticated
			if (User.Identity?.IsAuthenticated ?? false)
			{
				var accountId = GetCurrentLoginAccountId();
				ViewData["Account_Role"] = await _roleService.GetRoleById(accountId);
			}
			else
			{
				ViewData["Account_Role"] = "Guest";
			}

			await base.OnActionExecutionAsync(context, next);
		 }

		// SELECT: Get current login account ID method
		[HttpGet]
		public int GetCurrentLoginAccountId()
		{
			return _accountService.GetLoginAccountId(User);
		}
	}

}
