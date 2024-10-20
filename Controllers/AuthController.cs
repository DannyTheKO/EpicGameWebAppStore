using Domain.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace EpicGameWebAppStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authenticationServices;

        public AuthController(IAuthenticationService authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (await _authenticationServices.ValidateUserCredentialAsync(username, password))
            {
                var token = await _authenticationServices.GenerateTokenAsync(username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
