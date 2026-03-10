using BlazorWasm.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorWasm.Services;

/// <summary>
/// In-memory Compare service – holds up to 3 products for side-by-side comparison.
/// </summary>
public class CompareStateService
{
    private const string StorageKey = "avrang_compare";
    private readonly IJSRuntime _jsRuntime;
    private readonly List<Product> _items = new();
    private bool _initialized;
    public event Action? OnChanged;

    public CompareStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

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

    public IReadOnlyList<Product> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public bool IsInCompare(int id) => _items.Any(p => p.Id == id);

    public void Add(Product product)
    {
        if (_items.Count >= 3 || _items.Any(p => p.Id == product.Id)) return;
        _items.Add(product);
        _ = SaveAsync();
        OnChanged?.Invoke();
    }

    public void Remove(int productId)
    {
        var item = _items.FirstOrDefault(p => p.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            _ = SaveAsync();
            OnChanged?.Invoke();
        }
    }

    public void Toggle(Product product)
    {
        if (IsInCompare(product.Id)) Remove(product.Id);
        else Add(product);
    }

    public void Clear()
    {
        _items.Clear();
        _ = SaveAsync();
        OnChanged?.Invoke();
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_items);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
