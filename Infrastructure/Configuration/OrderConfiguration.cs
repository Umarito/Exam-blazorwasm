using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.Phone).HasMaxLength(20);
        builder.Property(a => a.Status).HasDefaultValue(OrderStatus.Pending);
        builder.HasOne(a => a.User).WithMany(a => a.Orders).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}