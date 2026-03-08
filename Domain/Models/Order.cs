public class Order : BaseEntity
{
    public string UserId{get;set;}
    public ApplicationUser? User{get;set;}
    public decimal TotalAmount{get;set;}
    public OrderStatus Status{get;set;}=OrderStatus.Pending;
    public string DeliveryAddress{get;set;}=null!;
    public string Phone{get;set;}=null!;
    public List<OrderItem> Items{get;set;}=new();
}