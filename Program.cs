using Microsoft.EntityFrameworkCore;

// Application
using EpicGameWebAppStore.Application.Services;
using EpicGameWebAppStore.Application.Interfaces;

// Infrastructure
using EpicGameWebAppStore.Infrastructure.Repository;
using EpicGameWebAppStore.Infrastructure.DataAccess;

// Domain
using EpicGameWebAppStore.Domain.Repository;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// Add connection into Database
builder.Services.AddDbContext<EpicGameDBContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")));

// Add scoped into services
builder.Services.AddScoped<IGameServices, GameServices>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

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
