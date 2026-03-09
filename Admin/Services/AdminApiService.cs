using System.Net.Http.Json;
using Admin.Models;

namespace Admin.Services;

public class AdminApiService
{
    private readonly HttpClient _http;

    public AdminApiService(HttpClient http)
    {
        _http = http;
    }

    #region Products
    public async Task<List<Product>> GetProductsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Product>>("products?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var response = await _http.GetFromJsonAsync<Response<Product>>($"products/{id}");
        return response?.Data;
    }

    public async Task<bool> CreateProductAsync(Product product)
    {
        var response = await _http.PostAsJsonAsync("products", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var response = await _http.PutAsJsonAsync($"products/{product.Id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _http.DeleteAsync($"products/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Categories
    public async Task<List<Category>> GetCategoriesAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Category>>("categories?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        var response = await _http.PostAsJsonAsync("categories", category);
        return response.IsSuccessStatusCode;
    }
    #endregion

    #region Installments
    public async Task<List<Installment>> GetInstallmentsAsync()
    {
        var result = await _http.GetFromJsonAsync<PagedResult<Installment>>("installments?PageSize=100");
        return result?.Items ?? new();
    }

    public async Task<bool> CreateInstallmentAsync(Installment installment)
    {
        var response = await _http.PostAsJsonAsync("installments", installment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteInstallmentAsync(int id)
    {
        var response = await _http.DeleteAsync($"installments/{id}");
        return response.IsSuccessStatusCode;
    }
    #endregion
}
