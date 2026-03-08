using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName{get;set;}=null!;
    public UserRole Role{get;set;}
    public bool IsActive{get;set;}=true;
    public DateTime CreatedAt{get;set;}=DateTime.UtcNow;
    public Cart? Cart{get;set;}
    public List<Order> Orders{get;set;}=new();
    public List<Review> Reviews{get;set;}=new();
    public List<Wishlist> WishlistItems{get;set;}=new();
}