namespace BlazorWasm.Models;

public class ProductImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
    public Product? Product { get; set; }
}
