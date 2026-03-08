using System.Net.Http.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync(int page = 1, int pageSize = 20)
    {
        try
        {
            var response = await _httpClient.GetAsync($"products?page={page}&pageSize={pageSize}");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Error: {response.StatusCode}");
                return new List<Product>();
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {content}");

            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<Product>();
            }

            var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<Product>>();
            return result?.Items ?? new List<Product>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProductsAsync: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
                return result?.Data;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProductAsync: {ex.Message}");
            return null;
        }
    }
}

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public List<string> Description { get; set; } = new();
    public T? Data { get; set; }
}