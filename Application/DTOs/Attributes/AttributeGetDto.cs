public class AttributeGetDto
{
    public int Id{get;set;}
    public string Name{get;set;}=null!;
    public List<ProductAttribute> AttributeValues{get;set;}=new();
}