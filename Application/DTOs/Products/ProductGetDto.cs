public class ProductGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public CategoryGetDto? Category { get; set; }
    public int BrandId { get; set; }
    public BrandGetDto? Brand { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsFeatured { get; set; }
    public string? ImageUrl { get; set; }
    public List<ProductImageGetDto>? Images { get; set; }
}
