using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");
        builder.HasKey(a => a.Id);
        builder.HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
        builder.HasOne(a => a.Product).WithMany().HasForeignKey(a => a.ProductId);
        builder.HasOne(a => a.User).WithMany(u => u.WishlistItems).HasForeignKey(a => a.UserId);
    }
}