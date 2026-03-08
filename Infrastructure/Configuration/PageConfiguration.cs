using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PageConfiguration : IEntityTypeConfiguration<PageSMS>
{
    public void Configure(EntityTypeBuilder<PageSMS> builder)
    {
        builder.ToTable("Pages");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.IsPublished).HasDefaultValueSql("false");
        builder.Property(a => a.Title).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Content).HasMaxLength(200);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}