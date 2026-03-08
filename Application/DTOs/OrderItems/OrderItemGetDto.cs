
public class OrderItemGetDto
{
    public int Id{get;set;}
    public int Quantity{get;set;}
    public decimal UnitPrice{get;set;}
    public ProductGetDto? Product{get;set;}
}