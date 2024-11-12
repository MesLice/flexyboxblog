using FlexyboxBlogRazor.Client;
using FlexyboxShared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using MudBlazor.Services;
using System.Net;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();

// Register the custom CookieHandler for managing cookies
builder.Services.AddTransient<CookieHandler>();

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri("https://localhost:7267/"))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

/*
// configure client for auth interactions
builder.Services.AddHttpClient(
    "Auth",
    opt => opt.BaseAddress = new Uri("https://localhost:7267/"))
.AddHttpMessageHandler<CookieHandler>();
*/

builder.Services.AddScoped<BlogPostService>();

await builder.Build().RunAsync();