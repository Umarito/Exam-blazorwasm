public class Cart : BaseEntity
{
    public string UserId{get;set;}
    public ApplicationUser? User{get;set;}
    public List<CartItem> Items{get;set;}=new();
}