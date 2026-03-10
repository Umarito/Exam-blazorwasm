public class CategoryGetDto
{
    public int Id{get;set;}
    public string Name{get;set;}=null!;
    public string Slug{get;set;}=null!;
    public string Description{get;set;}=null!;
    public int? ParentCategoryId{get;set;}
    public List<ProductGetDto>? Products{get;set;}
}
