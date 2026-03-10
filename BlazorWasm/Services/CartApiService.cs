using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class CartApiService
{
    private readonly HttpClient _httpClient;

    public CartApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get cart items for current user
    public async Task<List<CartItem>> GetMyCartItemsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("cartitems");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            
            if (doc.RootElement.TryGetProperty("data", out var data))
                return JsonSerializer.Deserialize<List<CartItem>>(data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            
            return JsonSerializer.Deserialize<List<CartItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch { return new(); }
    }

    // Add item to cart
    public async Task<bool> AddToCartAsync(int productId, int quantity = 1)
    {
        try
        {
            var request = new { productId, quantity };
            var response = await _httpClient.PostAsJsonAsync("cartitems", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // Update cart item quantity
    public async Task<bool> UpdateCartItemAsync(int cartItemId, int quantity)
    {
        try
        {
            var request = new { quantity };
            var response = await _httpClient.PutAsJsonAsync($"cartitems/{cartItemId}", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // Remove item from cart
    public async Task<bool> RemoveFromCartAsync(int cartItemId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"cartitems/{cartItemId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    // Clear entire cart
    public async Task<bool> ClearCartAsync()
    {
        try
        {
            var response = await _httpClient.DeleteAsync("carts");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
