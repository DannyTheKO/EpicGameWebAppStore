using Microsoft.EntityFrameworkCore;

// Application
using EpicGameWebAppStore.Application.Services;
using EpicGameWebAppStore.Application.Interfaces;

// Infrastructure
using EpicGameWebAppStore.Infrastructure.Repository;
using EpicGameWebAppStore.Infrastructure.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add connection into Database
builder.Services.AddDbContext<EpicGameDBContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Default")));

// Add scoped into services
builder.Services.AddScoped<IGameServices, GameServices>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
