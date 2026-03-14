public class OrderGetDto
{
    public int Id{get;set;}
    public DateTime CreatedAt{get;set;}
    public decimal TotalAmount{get;set;}
    public string Status{get;set;} = null!;
    public string DeliveryAddress{get;set;} = null!;
    public string Phone{get;set;} = null!;
    public UserGetDto? User{get;set;}
    public List<OrderItemGetDto>? Items{get;set;}
}
