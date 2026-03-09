using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWasm;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorWasm.Auth;
using BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point to backend API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5094/api/")
});

// Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<OrderApiService>();
builder.Services.AddScoped<InstallmentApiService>();
builder.Services.AddScoped<CartStateService>();
builder.Services.AddScoped<CompareStateService>();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();
