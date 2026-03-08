public class CartItemGetDto
{
    public int Id {get;set;}
    public int Quantity {get;set;}
    public ProductGetDto? Product {get;set;}
}