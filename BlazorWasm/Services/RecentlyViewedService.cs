using System.Text.Json;
using BlazorWasm.Models;
using Microsoft.JSInterop;

namespace BlazorWasm.Services;

public class RecentlyViewedService
{
    private const string StorageKey = "avrang_recently_viewed";
    private readonly IJSRuntime _jsRuntime;
    private readonly List<Product> _items = new();
    private bool _initialized;

    public event Action? OnChanged;

    public RecentlyViewedService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public IReadOnlyList<Product> Items => _items.AsReadOnly();

    public async Task InitializeAsync()
    {
        if (_initialized) return;
        _initialized = true;

        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrWhiteSpace(json))
        {
            var items = JsonSerializer.Deserialize<List<Product>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
        OnChanged?.Invoke();
    }

    public async Task AddAsync(Product product)
    {
        if (product == null) return;

        _items.RemoveAll(p => p.Id == product.Id);
        _items.Insert(0, product);

        if (_items.Count > 12)
            _items.RemoveRange(12, _items.Count - 12);

        await SaveAsync();
        OnChanged?.Invoke();
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_items);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
