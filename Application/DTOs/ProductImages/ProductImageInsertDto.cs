public class ProductImageInsertDto
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
}
