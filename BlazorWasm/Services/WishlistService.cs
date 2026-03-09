using BlazorWasm.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorWasm.Services;

/// <summary>
/// Manages wishlist items – persists to localStorage (works for guests too).
/// When logged in, could be synced to backend, but localStorage ensures persistence across refreshes.
/// </summary>
public class WishlistService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "avrang_wishlist";
    private List<Product> _items = new();

    public event Action? OnChanged;

    public WishlistService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            _items = JsonSerializer.Deserialize<List<Product>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }

    public IReadOnlyList<Product> Items => _items.AsReadOnly();
    public int Count => _items.Count;

    public bool IsInWishlist(int productId) => _items.Any(p => p.Id == productId);

    public async Task AddAsync(Product product)
    {
        if (!_items.Any(p => p.Id == product.Id))
        {
            _items.Add(product);
            await SaveAsync();
            OnChanged?.Invoke();
        }
    }

    public async Task RemoveAsync(int productId)
    {
        var item = _items.FirstOrDefault(p => p.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            await SaveAsync();
            OnChanged?.Invoke();
        }
    }

    public async Task ToggleAsync(Product product)
    {
        if (IsInWishlist(product.Id))
            await RemoveAsync(product.Id);
        else
            await AddAsync(product);
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_items);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
