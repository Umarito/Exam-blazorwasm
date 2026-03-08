public class ProductImage : BaseEntity
{
    public int ProductId{get;set;}
    public Product? Product{get;set;}
    public string ImageUrl{get;set;}=null!;
    public bool IsMain{get;set;}=false;
}