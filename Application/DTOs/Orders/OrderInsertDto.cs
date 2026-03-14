public class OrderInsertDto
{
    public string UserId{get;set;}
    public decimal TotalAmount{get;set;}
    public string DeliveryAddress{get;set;} = null!;
    public string Phone{get;set;} = null!;
}
