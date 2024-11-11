using FlexyboxShared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();

// Add API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7267")
});

// Add API Services for dependency injection
builder.Services.AddScoped<BlogPostService>();

await builder.Build().RunAsync();