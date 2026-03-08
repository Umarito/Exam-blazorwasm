using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.IsActive).HasDefaultValueSql("true");
        builder.Property(a => a.Description).HasMaxLength(300);
        builder.Property(a => a.Slug).HasMaxLength(200);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.HasOne(c => c.ParentCategory).WithMany(c => c.SubCategories).HasForeignKey(c => c.ParentCategoryId);
    }
}