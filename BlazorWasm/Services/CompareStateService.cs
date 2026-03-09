using BlazorWasm.Models;

namespace BlazorWasm.Services;

/// <summary>
/// In-memory Compare service – holds up to 3 products for side-by-side comparison.
/// </summary>
public class CompareStateService
{
    private readonly List<Product> _items = new();
    public event Action? OnChanged;

    public IReadOnlyList<Product> Items => _items;
    public int Count => _items.Count;
    public bool IsInCompare(int id) => _items.Any(p => p.Id == id);

    public void Add(Product product)
    {
        if (_items.Count >= 3 || _items.Any(p => p.Id == product.Id)) return;
        _items.Add(product);
        OnChanged?.Invoke();
    }

    public void Remove(int productId)
    {
        var item = _items.FirstOrDefault(p => p.Id == productId);
        if (item != null) { _items.Remove(item); OnChanged?.Invoke(); }
    }

    public void Toggle(Product product)
    {
        if (IsInCompare(product.Id)) Remove(product.Id);
        else Add(product);
    }

    public void Clear() { _items.Clear(); OnChanged?.Invoke(); }
}
