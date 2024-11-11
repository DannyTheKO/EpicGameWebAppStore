using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

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
	            var currentLoginAccountDetail = await accountServices.GetAccountById(accountId);
	            var currentLoginRole = await roleServices.GetRoleById(currentLoginAccountDetail.RoleId.Value);

                context.Items["Account_Username"] = currentLoginAccountDetail.Username;
                context.Items["Account_Role"] = currentLoginRole.Name;
                context.Items["Account_Permission"] = currentLoginRole.Permission;

                context.Response.Headers.Add("X-User-Authenticated", context.User.Identity?.IsAuthenticated.ToString());
                context.Response.Headers.Add("X-User-Name", currentLoginAccountDetail.Username);
                context.Response.Headers.Add("X-User-Role", currentLoginRole.Name);
                context.Response.Headers.Add("X-User-Permission", JsonSerializer.Serialize(currentLoginRole.Permission));
            }
            else
            {
                context.Items["Account_Role"] = "Guest";
                context.Items["Account_Username"] = string.Empty;

                context.Response.Headers.Add("X-User-Authenticated", context.User.Identity?.IsAuthenticated.ToString());
                context.Response.Headers.Add("X-User-Role", "Guest");
                context.Response.Headers.Add("X-User-Name", string.Empty);
                context.Response.Headers.Add("X-User-Permission", string.Empty);
            }
                await _next(context);
        }
    }
}