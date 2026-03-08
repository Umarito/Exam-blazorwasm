public class Category : BaseEntity
{
    public string Name{get;set;}=null!;
    public string Slug{get;set;}=null!;
    public string? Description{get;set;}
    public int? ParentCategoryId{get;set;}
    public Category? ParentCategory{get;set;}
    public bool IsActive{get;set;}=true;
    public List<Category> SubCategories{get;set;}=new();
    public List<Product> Products{get;set;}=new();
}