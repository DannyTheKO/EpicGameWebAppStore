using System.Text;
using Application.Interfaces;
using Application.Services;
using DataAccess.EpicGame;
using Domain.Repository;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add connection into Database
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrEmpty(connectionString)) // This will check if the Default connection string
    throw new InvalidOperationException("Connection string 'Default' is not found.");

builder.Services.AddDbContext<EpicGameDbContext>(options =>
    options.UseMySQL(connectionString));


// == Service registrations ==
// Authentication && Authorization
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IAuthorizationServices, AuthorizationServices>();

// Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
	});

builder.Services.AddCors(options =>
{
	options.AddPolicy("ReactPolicy",
	builder => builder
		.WithOrigins("http://localhost:3000")
		.AllowCredentials()
		.AllowAnyMethod()
			.AllowAnyHeader());
});

// Account
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Role
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Game
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

// Account Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// Cart Service
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddScoped<ICartdetailService, CartdetailService>();
builder.Services.AddScoped<ICartdetailRepository, CartdetailRepository>();

// PaymentMethod Service
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

// Genre Service
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Publisher Service
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();


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
app.UseCors("ReactPolicy");
app.UseAuthentication();
app.UseMiddleware<AuthHeader>();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();