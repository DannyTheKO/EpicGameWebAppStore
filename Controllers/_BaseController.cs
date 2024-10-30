using System.Security.Claims;
using Application.Interfaces;
using Domain.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EpicGameWebAppStore.Controllers
{
// Location: Controllers/_BaseController.cs
	public abstract class _BaseController : Controller
	{
		private readonly IAuthorizationServices _authorizationServices;
		private readonly IAuthenticationServices _authenticationServices;

		protected _BaseController(
			IAuthenticationServices authenticationServices,
			IAuthorizationServices authorizationServices)
		{
			_authenticationServices = authenticationServices;
			_authorizationServices = authorizationServices;
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Existing view data setup code
			ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;
			ViewData["Account_Username"] = User.Identity?.Name;

			if (User.Identity?.IsAuthenticated ?? false)
			{
				var account = await _authenticationServices.GetAccountByUsername(User.Identity?.Name);
				if (account != null)
				{
					ViewData["Account_Role"] = await _authorizationServices.GetUserRole(account.AccountId);
				}
				else
				{
					ViewData["Account_Role"] = "Guest";
				}
			}
			else
			{
				ViewData["Account_Role"] = "Guest";
			}

			await base.OnActionExecutionAsync(context, next);
		}
	}

}
