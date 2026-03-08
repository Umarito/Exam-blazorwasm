using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(g => g.Id);
        builder.HasOne(a => a.Cart).WithMany(a => a.Items).HasForeignKey(a => a.CartId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Product).WithMany(a => a.CartItems).HasForeignKey(a => a.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}