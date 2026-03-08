using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(g => g.Id);
        builder.HasOne(a => a.Order).WithMany(a => a.Items).HasForeignKey(a => a.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Product).WithMany(a => a.OrderItems).HasForeignKey(a => a.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}