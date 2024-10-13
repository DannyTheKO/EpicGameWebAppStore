// Domain
using EpicGameWebAppStore.Domain.Entities;
using EpicGameWebAppStore.Infrastructure.DataAccess; // Thêm không gian tên cho DbContext
using Microsoft.EntityFrameworkCore;

// Application



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình kết nối đến cơ sở dữ liệu
builder.Services.AddDbContext<EpicgamewebappContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm các dịch vụ khác nếu cần
// builder.Services.AddScoped<IYourService, YourService>(); // Ví dụ thêm dịch vụ


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
