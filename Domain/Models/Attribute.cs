public class Attribute : BaseEntity
{
    public string Name{get;set;}=null!;
    public List<ProductAttribute> AttributeValues{get;set;}=new();
}