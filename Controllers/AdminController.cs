using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EpicGameWebAppStore.Controllers
{
	public class AdminController : Controller
	{
		private readonly IAuthorizationServices _authorizationServices;
		private readonly IAuthenticationServices _authenticationServices;

		public AdminController(IAuthorizationServices authorizationServices,
			IAuthenticationServices authenticationServices)
		{
			_authorizationServices = authorizationServices;
			_authenticationServices = authenticationServices;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
