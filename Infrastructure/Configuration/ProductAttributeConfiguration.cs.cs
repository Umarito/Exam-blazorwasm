using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("ProductAttributes");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Value).IsRequired().HasMaxLength(500);
        builder.HasOne(a => a.Product).WithMany(p => p.AttributeValues).HasForeignKey(a => a.ProductId);
        builder.HasOne(a => a.Attribute).WithMany(a => a.AttributeValues).HasForeignKey(a => a.AttributeId);
    }
}