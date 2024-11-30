// Path: Controllers/_BaseController.cs

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
	public class _BaseController : ControllerBase
	{
		protected readonly IAuthorizationServices _authorizationServices;

		public _BaseController(IAuthorizationServices authorizationServices)
		{
			_authorizationServices = authorizationServices;
		}

		protected async Task<ActionResult> CheckPermission(string requiredPermission)
		{
			var accountDetails = GetCurrentDetailAccount();
			var hasPermission = await _authorizationServices.UserHasPermission(accountDetails.AccountId, requiredPermission);
    
			// Returns null if hasPermission is true, otherwise returns the AccessDenied redirect
			return hasPermission ? null : RedirectToAction("AccessDenied", "Auth");
		}



		protected (int AccountId, string Username, string Role, string Email, string IsActive, string Permissions) GetCurrentDetailAccount()
		{
			return (
				AccountId: int.Parse(HttpContext.Items["AccountId"]?.ToString() ?? "0"),
				Username: HttpContext.Items["Username"]?.ToString(),
				Role: HttpContext.Items["Role"]?.ToString(),
				Email: HttpContext.Items["Email"]?.ToString(),
				IsActive: HttpContext.Items["IsActive"]?.ToString(),
				Permissions: HttpContext.Items["Permission"]?.ToString()
			);
		}
	}
}