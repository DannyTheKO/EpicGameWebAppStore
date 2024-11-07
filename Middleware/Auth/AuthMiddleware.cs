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
            var accountServices = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var roleServices = scope.ServiceProvider.GetRequiredService<IRoleService>();

            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var accountId = accountServices.GetLoginAccountId(context.User);

                context.Items["Account_Role"] = await roleServices.GetRoleById(accountId);
                context.Response.Headers.Add("X-User-Role",
                    await roleServices.GetRoleById(accountId));
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
