using BlazorWasm.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorWasm.Services;

public class CartStateService
{
    private const string StorageKey = "avrang_cart";
    private readonly IJSRuntime _jsRuntime;
    private readonly List<CartItem> _items = new();
    private bool _initialized;
    public event Action? OnChanged;

    public CartStateService(IJSRuntime jsRuntime)
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
            var items = JsonSerializer.Deserialize<List<CartItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
        OnChanged?.Invoke();
    }

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int Count => _items.Sum(i => i.Quantity);
    public decimal Total => _items.Sum(i => i.Product.Price * i.Quantity);

    public void AddToCart(Product product)
    {
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null) existing.Quantity++;
        else _items.Add(new CartItem { Product = product, Quantity = 1 });
        _ = SaveAsync();
        OnChanged?.Invoke();
    }

    public void AddToCart(Product product, int quantity)
    {
        if (quantity <= 0) return;
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null) existing.Quantity += quantity;
        else _items.Add(new CartItem { Product = product, Quantity = quantity });
        _ = SaveAsync();
        OnChanged?.Invoke();
    }

    public void Remove(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            _ = SaveAsync();
            OnChanged?.Invoke();
        }
    }

    public void SetQuantity(int productId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item == null) return;
        if (quantity <= 0)
        {
            _items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }
        _ = SaveAsync();
        OnChanged?.Invoke();
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
