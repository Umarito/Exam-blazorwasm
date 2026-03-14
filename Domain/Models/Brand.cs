public class Brand : BaseEntity
{
    public string Name{get;set;} = null!;
    public string? LogoUrl{get;set;}
    public string Slug{get;set;} = null!;
    public List<Product> Products{get;set;} = new();
    public Brand(){}
    public Brand(int id,string name, string slug, string? logoUrl = null)
    {
        Id = id;
        Name = name;
        Slug = slug;
        LogoUrl = logoUrl;
    }
}