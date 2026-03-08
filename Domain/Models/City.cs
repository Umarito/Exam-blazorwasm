public class City : BaseEntity
{
    public string Name{get;set;}=null!;
    public List<StoreLocation> StoreLocations{get;set;}=new();
}