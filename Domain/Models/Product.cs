public class Product : BaseEntity
{
    public string Name{get;set;}=null!;
    public string? Description{get;set;}
    public decimal Price{get;set;}
    public int StockQuantity{get;set;}=0;
    public int CategoryId{get;set;}
    public Category? Category{get;set;}
    public decimal OldPrice{get;set;}
    public int BrandId{get;set;}
    public Brand? Brand{get;set;}
    public bool IsActive{get;set;}=true;
    public List<ProductImage> Images{get;set;}=new();
    public List<CartItem> CartItems{get;set;}=new();
    public List<OrderItem> OrderItems{get;set;}=new();
    public List<ProductAttribute> AttributeValues{get;set;}=new();
    public List<Review> Reviews{get;set;}=new();
}