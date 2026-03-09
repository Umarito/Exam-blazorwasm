public class ProductUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public bool IsFeatured { get; set; }
    public string? ImageUrl { get; set; }
}