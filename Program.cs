using Microsoft.EntityFrameworkCore;

// Application
using Application.Services;
using Application.Interfaces;

// Infrastructure
using Infrastructure.DataAccess;
using Infrastructure.Repository;

// Domain
using Domain.Repository;
using Domain.Authentication;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// Add connection into Database
builder.Services.AddDbContext<EpicGameDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")!));

// == Add scoped into services ==

// Authentication Service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
