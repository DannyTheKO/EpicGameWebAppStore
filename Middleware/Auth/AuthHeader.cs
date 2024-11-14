using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Middleware.Auth;

public class AuthHeader
{
	private readonly RequestDelegate _next;
	private readonly IServiceScopeFactory _scopeFactory;

	public AuthHeader(RequestDelegate next, IServiceScopeFactory scopeFactory)
	{
		_next = next;
		_scopeFactory = scopeFactory;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		
		using var scope = _scopeFactory.CreateScope();
		
		// Create a scope to resolve IAccountService and IRoleService
		var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
		var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();


		if (token != null)
		{
			// Get claims from token
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

			if (jsonToken != null)
			{
				// Extract account information from claims
				var accountId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
				var username = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
				var role = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
				var email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
				var permission = await roleService.GetRoleByName(role);

				// Add to HttpContext for easy access in controllers
				context.Items["AccountId"] = accountId;
				context.Items["Username"] = username;
				context.Items["Role"] = role;
				context.Items["Email"] = email;
				context.Items["Permission"] = permission.Permission;
			}
		}

		await _next(context);
	}

}
