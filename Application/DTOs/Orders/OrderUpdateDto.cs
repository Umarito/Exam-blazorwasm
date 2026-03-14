
public class OrderUpdateDto
{
    public int Id{get;set;}
    public string UserId{get;set;}
    public decimal TotalAmount{get;set;}
    public string DeliveryAddress{get;set;} = null!;
    public string Phone{get;set;} = null!;
    public string Status{get;set;} = null!;
}
