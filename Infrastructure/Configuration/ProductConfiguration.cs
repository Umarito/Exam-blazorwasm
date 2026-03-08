using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.IsActive).HasDefaultValueSql("true");
        builder.HasOne(a => a.Category).WithMany(a => a.Products).HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Description).HasMaxLength(200);
        builder.Property(a => a.StockQuantity).HasDefaultValue(0);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}