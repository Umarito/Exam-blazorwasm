using System.Net.Http.Json;
using BlazorWasm.Models;
using Microsoft.JSInterop;

namespace BlazorWasm.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", model);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result?.Data?.Token != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Data.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.Token);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> RegisterAsync(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", model);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
    }

    public async Task InitializeAsync()
    {
        var token = await GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}

public class AuthResponse
{
    public int StatusCode { get; set; }
    public List<string> Description { get; set; } = new();
    public AuthData? Data { get; set; }
}

public class AuthData
{
    public string Token { get; set; } = null!;
}