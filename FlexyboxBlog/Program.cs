using FlexyboxBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Cookie Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Cookies only sent over HTTPS
    options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin requests
    options.Cookie.Name = "BlogAuthCookie";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7212",          // Localhost (for local development)
                "https://localhost:7267",          // Another development port
                "https://localhost:443",
                "https://flexyboxrazor:7212",
                "https://flexyboxrazor:7267",
                "https://flexyboxrazor:443",
                "https://flexyboxblog:7212",
                "https://flexyboxblog:7267",
                "https://flexyboxblog:443")       // Docker container name (for Docker deployment)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// HTTPS redirection should come before CORS
app.UseHttpsRedirection();

// Apply CORS policy before Authentication/Authorization
app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Map Identity API endpoints with CORS
app.MapIdentityApi<IdentityUser>().RequireCors("AllowBlazorClient");

// Map logout endpoint with CORS
app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager, [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
.RequireAuthorization()
.RequireCors("AllowBlazorClient");

// Map controller endpoints
app.MapControllers().RequireCors("AllowBlazorClient");

app.Run();