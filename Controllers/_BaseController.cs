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
			// Set authentication status and username for all views
			ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;
			ViewData["Account_Username"] = User.Identity?.Name;

			// Add role information if user is authenticated
			if (User.Identity?.IsAuthenticated ?? false)
			{
				var accountId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
				ViewData["Account_Role"] = await _authorizationServices.GetRoleById(accountId);
			}
			else
			{
				ViewData["Account_Role"] = "Guest";
			}

			await base.OnActionExecutionAsync(context, next);
		 }

		// SELECT: Get current login account ID method
		public int GetCurrentLoginAccountId()
		{
			return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
		}
	}

}
