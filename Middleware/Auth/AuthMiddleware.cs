using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public AuthMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var authorizationServices = scope.ServiceProvider.GetRequiredService<IAuthorizationServices>();
            var authenticationServices = scope.ServiceProvider.GetRequiredService<IAuthenticationServices>();

            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var accountId = authenticationServices.GetCurrentLoginAccountId(context.User);

                context.Items["Account_Role"] = await authorizationServices.GetRoleById(accountId);
                context.Response.Headers.Add("X-User-Role",
                    await authorizationServices.GetRoleById(accountId));
            }
            else
            {
                context.Items["Account_Role"] = "Guest";
                context.Response.Headers.Add("X-User-Role", "Guest");
            }

            await _next(context);
        }
    }
}
