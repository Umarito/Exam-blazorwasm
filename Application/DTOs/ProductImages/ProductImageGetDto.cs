public class ProductImageGetDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public bool IsMain { get; set; }
    public ProductGetDto? Product { get; set; }
}
