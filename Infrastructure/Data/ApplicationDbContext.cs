using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
    // public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ContactMessages> ContactMessages => Set<ContactMessages>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<PageSMS> PageSMSs => Set<PageSMS>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<StoreLocation> StoreLocations => Set<StoreLocation>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
    public DbSet<Installment> Installments => Set<Installment>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Attribute> Attributes => Set<Attribute>();
}