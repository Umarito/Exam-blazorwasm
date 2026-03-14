using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class OrderApiService
{
    private readonly HttpClient _httpClient;

    public OrderApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrderModel>> GetMyOrdersAsync()
    {
        try
        {
            return await GetMyOrdersAsync(null);
        }
        catch { return new(); }
    }

    public async Task<List<OrderModel>> GetMyOrdersAsync(string? userId)
    {
        try
        {
            var url = string.IsNullOrWhiteSpace(userId) ? "orders" : $"orders?UserId={Uri.EscapeDataString(userId)}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("items", out var items))
                return MapOrders(JsonSerializer.Deserialize<List<OrderModel>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new());
            if (doc.RootElement.TryGetProperty("data", out var data))
                return MapOrders(JsonSerializer.Deserialize<List<OrderModel>>(data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new());
            return MapOrders(JsonSerializer.Deserialize<List<OrderModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new());
        }
        catch { return new(); }
    }

    private static List<OrderModel> MapOrders(List<OrderModel> orders)
    {
        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                if (item.Product != null)
                {
                    item.ProductName = item.Product.Name;
                    item.Price = item.UnitPrice > 0 ? item.UnitPrice : item.Product.Price;
                }
            }
        }
        return orders;
    }

    public async Task<int?> CreateOrderAsync(string userId, decimal totalAmount, string deliveryAddress, string phone)
    {
        try
        {
            var request = new { userId, totalAmount, deliveryAddress, phone };
            var response = await _httpClient.PostAsJsonAsync("orders", request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("data", out var data) && data.TryGetInt32(out var id))
                return id;

            return null;
        }
        catch { return null; }
    }

    public async Task<bool> CreateOrderItemAsync(int orderId, int productId, int quantity)
    {
        try
        {
            var request = new { orderId, productId, quantity };
            var response = await _httpClient.PostAsJsonAsync("orderitems", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
