using Home_Expert.DependencyInjections;
using Home_Expert.Models;
using Home_Expert.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Session Configuration
// ==========================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".HomeExpert.Session"; // اسم مميز للـ Cookie
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// ==========================================
// 2. Services Registration
// ==========================================
builder.Services.AddScoped<IOtpService, OtpService>();

// ==========================================
// 3. Controllers & Views
// ==========================================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ✅ إضافة: دعم API Controllers مع JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // يخلي الـ JSON يحافظ على أسماء الـ Properties كما هي
        options.JsonSerializerOptions.WriteIndented = true; // JSON منسق (للتطوير)
    });

// ==========================================
// 4. Database Context
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 5. Localization
// ==========================================
builder.Services.AddLocalizationDependencyInjection();

// ==========================================
// 6. Identity Configuration
// ==========================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password Requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User Requirements
    options.User.RequireUniqueEmail = true;

    // Sign In Requirements
    options.SignIn.RequireConfirmedEmail = false; // لو بدك تفعيل الإيميل، غيرها لـ true
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Lockout Settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ==========================================
// 7. Cookie Authentication Configuration
// ==========================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // مدة بقاء الـ Cookie
    options.SlidingExpiration = true; // تجديد تلقائي
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// ==========================================
// 8. CORS (لو بدك تستخدم API من frontend منفصل)
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==========================================
// 9. Email Settings Validation
// ==========================================
var emailSettings = builder.Configuration.GetSection("EmailSettings");
if (string.IsNullOrEmpty(emailSettings["SmtpPassword"]) || emailSettings["SmtpPassword"] == "your-app-password")
{
    Console.WriteLine("⚠️ Warning: Email settings are not configured!");
    Console.WriteLine("Please update appsettings.json with your Gmail App Password");
    Console.WriteLine("To generate an App Password:");
    Console.WriteLine("1. Go to https://myaccount.google.com/security");
    Console.WriteLine("2. Enable 2-Step Verification");
    Console.WriteLine("3. Generate App Password for 'Mail'");
}

// ==========================================
// 10. Logging Configuration (اختياري)
// ==========================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// ==========================================
// Middleware Pipeline
// ==========================================

// 1. Session (قبل كل شي!)
app.UseSession();

// 2. Localization
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

// 3. Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// 4. HTTPS Redirection
app.UseHttpsRedirection();

// 5. Static Files
app.UseStaticFiles();

// 6. Routing
app.UseRouting();

// 7. CORS (لو فعّلته)
// app.UseCors("AllowAll");

// 8. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ==========================================
// Endpoints Configuration
// ==========================================

// Map Controllers (للـ API)
app.MapControllers();

// Map MVC Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Registration}/{id?}");

// Map Razor Pages
app.MapRazorPages();

// ==========================================
// Database Initialization & Seeding
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // تطبيق Migrations تلقائياً
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // إنشاء Roles الافتراضية
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "Vendor", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"✅ Role '{role}' created successfully");
            }
        }

        Console.WriteLine("✅ Database initialized successfully");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ An error occurred while migrating or seeding the database");
    }
}

app.Run();