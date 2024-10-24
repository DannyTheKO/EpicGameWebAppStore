using Microsoft.EntityFrameworkCore;

// Application
using Application.Interfaces;
using Application.Services;

// Infrastructure
using Infrastructure.Repository;
using DataAccess.EpicGame;

// Domain
using Domain.Repository;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// == Add scoped into services ==

// Add connection into Database
builder.Services.AddDbContext<EpicGameDbContext>(options =>
	options.UseMySQL(builder.Configuration.GetConnectionString("Default")!));

// Authentication Service
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddAuthentication("CookieAuth")
	.AddCookie("CookieAuth", config =>
	{
		config.Cookie.Name = "UserLoginCookie";
		config.LoginPath = "/Auth/LoginPage";
	});

// Authorization Service


// Game Service
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

// Account Service

// Discount Service

// Genre Service

// Publisher Service

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");

	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	"default",
	"{controller=Home}/{action=Index}/{id?}");

app.Run();