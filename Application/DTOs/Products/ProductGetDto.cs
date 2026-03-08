
public class ProductGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public CategoryGetDto? Category { get; set; }
    public List<ProductImageGetDto>? Images { get; set; }
}