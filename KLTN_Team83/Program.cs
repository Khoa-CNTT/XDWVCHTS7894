using KLTN_Team83;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using KLTN_Team83.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "1417627842738603";
    options.AppSecret = "6708666c56caf7f92456bc24810058f7";
});

builder.Services.AddRazorPages();

//Interface và Repository
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

//Add services appsetting
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection("Gemini")
);

// Đăng ký IHttpClientFactory
builder.Services.AddHttpClient<GeminiService>();

// --- Cấu hình Session ---
// 1. Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Lưu session trong bộ nhớ (phù hợp cho dev/ứng dụng nhỏ)
                                              // Cân nhắc dùng SQL Server hoặc Redis cho production
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn nếu không hoạt động
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập được bởi server
    options.Cookie.IsEssential = true; // Cần thiết cho GDPR compliance
});
// -------------------------

// Nạp cấu hình Gemini từ appsettings và các nguồn khác
builder.Configuration.AddUserSecrets<Program>(); // Cho development
builder.Configuration.AddEnvironmentVariables();

// --- (Tùy chọn) Đăng ký Gemini Service ---
//builder.Services.AddSingleton<GeminiService>(); // Hoặc AddScoped/AddTransient tùy nhu cầu

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
app.MapRazorPages();

// --- Kích hoạt Session Middleware ---
// Quan trọng: Phải đặt UseSession() trước UseEndpoints() hoặc MapControllerRoute()
app.UseSession();
// -----------------------------------


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

// Map API controllers (nếu bạn dùng [ApiController] attribute)
app.MapControllers(); // Đảm bảo có dòng này để map API controller

app.Run();
