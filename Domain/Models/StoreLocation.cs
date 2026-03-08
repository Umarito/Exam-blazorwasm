public class StoreLocation : BaseEntity
{
    public string Address{get;set;}=null!;
    public string WorkingHours{get;set;}=null!;
    public string MapCoordinates{get;set;}=null!;
    public int CityId{get;set;}
    public City? City{get;set;}
}