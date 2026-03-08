using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Comment).HasMaxLength(2000);
        builder.Property(a => a.Rating).IsRequired();
        builder.HasOne(a => a.Product).WithMany(p => p.Reviews).HasForeignKey(a => a.ProductId);
        builder.HasOne(a => a.User).WithMany(u => u.Reviews).HasForeignKey(a => a.UserId);
    }
}