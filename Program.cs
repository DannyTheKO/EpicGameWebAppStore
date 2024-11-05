using Application.Interfaces;
using Application.Services;
using DataAccess.EpicGame;
using Domain.Repository;
//using EpicGameWebAppStore.Infrastructure.Repository;
//using Infrastructure.DataAccess;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// == Add scoped into services ==

// Add connection into Database
builder.Services.AddDbContext<EpicGameDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")));


// Authentication Service
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Auth/LoginPage";
        config.AccessDeniedPath = "/Auth/AccessDenied";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });

// Authorization Service
builder.Services.AddScoped<IAuthorizationServices, AuthorizationServices>();

// Account And Role Service
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Game Service
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

// Account Service

// Discount Service

// Genre Service

// Publisher Service

// == Testing API ==
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Epic Game Web App Store API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Epic Game Web App Store API v1"));
}


//if (!app.Environment.IsDevelopment())
//{
//	app.UseExceptionHandler("/Home/Error");

//	app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();