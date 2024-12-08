using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Data;
using WebApplication1.IServices;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configure cookie settings
    options.Cookie.Name = "YourAppCookie"; // Set your cookie name
    options.Cookie.HttpOnly = true; // Prevent access to the cookie via JavaScript
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //the cookie is only sent over HTTPS if you're using it in production
    options.Cookie.SameSite = SameSiteMode.None; // Adjust this based on your use cas
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Set the cookie expiration time
    options.SlidingExpiration = true; // Refresh cookie expiration on every request
});

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

// Add other services
builder.Services.AddScoped<IUserOrganizationValidator, UserOrganizationValidator>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins("http://localhost:5173") // Allow only the React app's origin (replace if necessary)
            .AllowAnyMethod() // Allow any HTTP method
            .AllowAnyHeader() // Allow any headers
            .AllowCredentials(); // Allow cookies (credentials)
    });
});

builder.Services.AddControllers();

var app = builder.Build();
// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    await ModelBuilderExtensions.SeedAsync(userManager, roleManager, context);
}

// Enable Swagger in the app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 API v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowAnyOrigin");
app.MapIdentityApi<User>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
