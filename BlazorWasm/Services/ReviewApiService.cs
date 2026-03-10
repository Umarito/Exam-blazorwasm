using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class ReviewApiService
{
    private readonly HttpClient _httpClient;

    public ReviewApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get reviews for a product
    public async Task<List<ReviewModel>> GetProductReviewsAsync(int productId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"reviews?productId={productId}");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            
            if (doc.RootElement.TryGetProperty("items", out var items))
                return JsonSerializer.Deserialize<List<ReviewModel>>(items.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            
            return JsonSerializer.Deserialize<List<ReviewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch { return new(); }
    }

    // Add review
    public async Task<(bool Success, string? Error)> AddReviewAsync(int productId, int rating, string? comment)
    {
        try
        {
            var request = new { productId, rating, comment };
            var response = await _httpClient.PostAsJsonAsync("reviews", request);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    // Delete review
    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"reviews/{reviewId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}

public class ReviewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ProductId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
