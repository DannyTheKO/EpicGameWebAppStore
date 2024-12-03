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
		public class CurrentAccountDetails
		{
			public int AccountId { get; init; }
			public string? Username { get; init; }
			public string? Role { get; init; }
			public string? Email { get; init; }
			public string? IsActive { get; init; }
			public string? Permissions { get; init; }
		}

		protected CurrentAccountDetails GetCurrentDetailAccount()
		{
			return new CurrentAccountDetails
			{
				//in case something happened, for debug
				AccountId = HttpContext.Items["AccountId"] is string accountId ? int.Parse(accountId) : 0,
				Username = HttpContext.Items["Username"] as string,
				Role = HttpContext.Items["Role"] as string,
				Email = HttpContext.Items["Email"] as string,
				IsActive = HttpContext.Items["IsActive"] as string,
				Permissions = HttpContext.Items["Permission"] as string
			};
		}

		protected async Task<IActionResult> CheckPermission(string requiredPermission)
		{
			var permissionCheck = await _authorizationServices.UserHasPermission(GetCurrentDetailAccount().AccountId, requiredPermission);
			if (!permissionCheck)
			{
				return Redirect("Auth/AccessDenied");
			}

			return null; // this is a void return, but that fuck it, who cared anyway
		}

	}
}