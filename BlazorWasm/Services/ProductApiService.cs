using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class ProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync(int page = 1, int pageSize = 20)
    {
        try
        {
            var response = await _httpClient.GetAsync($"products?page={page}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode) return new List<Product>();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            // Handle PagedResult wrapper
            if (doc.RootElement.TryGetProperty("items", out var items))
            {
                return JsonSerializer.Deserialize<List<Product>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            // Handle direct list
            return JsonSerializer.Deserialize<List<Product>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetProductsAsync error: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"products/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("data", out var data))
            {
                return JsonSerializer.Deserialize<Product>(data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return JsonSerializer.Deserialize<Product>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetProductAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("items", out var items))
            {
                return JsonSerializer.Deserialize<List<Category>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            if (doc.RootElement.TryGetProperty("data", out var data))
            {
                return JsonSerializer.Deserialize<List<Category>>(data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            return JsonSerializer.Deserialize<List<Category>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<Brand>> GetBrandsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("brands");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("items", out var items))
            {
                return JsonSerializer.Deserialize<List<Brand>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            if (doc.RootElement.TryGetProperty("data", out var data))
            {
                return JsonSerializer.Deserialize<List<Brand>>(data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            return JsonSerializer.Deserialize<List<Brand>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch { return new(); }
    }
}
