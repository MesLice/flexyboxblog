using FlexyboxShared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();

// Add API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5023/")
});

// Add API Services
builder.Services.AddScoped<BlogPostService>();

await builder.Build().RunAsync();


await builder.Build().RunAsync();
