public class Brand : BaseEntity
{
    public string Name{get;set;} = null!;
    public string? LogoUrl{get;set;}
    public string Slug{get;set;} = null!;
    public List<Product> Products{get;set;} = new();
}