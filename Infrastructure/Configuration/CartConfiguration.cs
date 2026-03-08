using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        builder.HasKey(g => g.Id);
        builder.HasOne(a => a.User).WithOne(a => a.Cart).HasForeignKey<Cart>(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}