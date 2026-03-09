using System.Net.Http.Json;
using Microsoft.JSInterop;
using Admin.Auth;
using Admin.Models;

namespace Admin.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _jsRuntime;
    private readonly JwtAuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, IJSRuntime jsRuntime, JwtAuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _jsRuntime = jsRuntime;
        _authStateProvider = authStateProvider;
    }

    public async Task<(bool success, string? error)> LoginAsync(LoginDto model)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                    _authStateProvider.NotifyAuthStateChanged();
                    return (true, null);
                }
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, error ?? "Login failed");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _authStateProvider.NotifyAuthStateChanged();
    }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
}
