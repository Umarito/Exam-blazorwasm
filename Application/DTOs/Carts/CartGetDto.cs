
public class CartGetDto
{
    public int Id {get;set;}
    public UserGetDto? User {get;set;}
    public List<CartItemGetDto>? Items {get;set;}
}
