using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.IsMain).HasDefaultValueSql("false");
        builder.HasOne(a => a.Product).WithMany(a => a.Images).HasForeignKey(a => a.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}