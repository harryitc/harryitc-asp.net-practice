using FlowerShop.Models;
using FlowerShop.Models.Payment;
using FlowerShop.Repository;
using FlowerShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
 .AddDefaultTokenProviders()
 .AddDefaultUI()
 .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Đăng ký Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Đăng ký Service
builder.Services.AddHttpClient<IImageService, UnsplashService>();
builder.Services.AddScoped<ImageUploadService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddHttpContextAccessor();

// Đăng ký MoMo Payment Service
builder.Services.Configure<MoMoConfig>(builder.Configuration.GetSection("MoMo"));
builder.Services.AddHttpClient<MoMoPaymentService>();
builder.Services.AddScoped<MoMoPaymentService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseMiddleware<AuthorizationMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

// Thêm Session vào Middleware
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapStaticAssets();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();
app.UseEndpoints(endpoints =>
{
 endpoints.MapControllerRoute(
 name: "Admin",
 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
 endpoints.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
