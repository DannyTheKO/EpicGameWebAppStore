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
// Authentication & Authorization
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IAuthorizationServices, AuthorizationServices>();
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

// Account Related Service
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountGameService, AccountGameService>();
builder.Services.AddScoped<IAccountGameRepository, AccountGameRepository>();

// Shopping Related Service
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartdetailService, CartdetailService>();
builder.Services.AddScoped<ICartdetailRepository, CartdetailRepository>();

// Game Related Service
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IImageGameService, ImageGameService>();
builder.Services.AddScoped<IImageGameRepository, ImageGameRepository>();

// Other services
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();


// == Testing API ==
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "EpicGame Web API", Version = "v1" });
    
	// Add JWT Authentication
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
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
app.UseAntiforgery();

app.UseRouting();
app.UseCors("ReactPolicy");
app.UseAuthentication();
app.UseMiddleware<AuthHeader>();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}");

app.Run();