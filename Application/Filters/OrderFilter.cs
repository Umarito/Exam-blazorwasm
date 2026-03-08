public class OrderFilter
{
    public decimal TotalAmount{get;set;}
    public OrderStatus Status{get;set;}
    public string? DeliveryAddress{get;set;}
    public string Phone{get;set;}=null!;
}