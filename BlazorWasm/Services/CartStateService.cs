using BlazorWasm.Models;

namespace BlazorWasm.Services;

public class CartStateService
{
    private readonly List<CartItem> _items = new();
    public event Action? OnChanged;

    public IReadOnlyList<CartItem> Items => _items;
    public int Count => _items.Sum(i => i.Quantity);
    public decimal Total => _items.Sum(i => i.Product.Price * i.Quantity);

    public void AddToCart(Product product)
    {
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null) existing.Quantity++;
        else _items.Add(new CartItem { Product = product, Quantity = 1 });
        OnChanged?.Invoke();
    }

    public void Remove(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null) { _items.Remove(item); OnChanged?.Invoke(); }
    }

    public void Clear() { _items.Clear(); OnChanged?.Invoke(); }
}
