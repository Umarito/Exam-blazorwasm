using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessages>
{
    public void Configure(EntityTypeBuilder<ContactMessages> builder)
    {
        builder.ToTable("ContactMessages");
        builder.HasKey(g => g.Id);
        builder.HasIndex(a=>a.Email).IsUnique();
        builder.Property(a => a.FullName).HasMaxLength(100);
        builder.Property(a => a.Message).HasMaxLength(500);
        builder.Property(a => a.IsRead).HasDefaultValueSql("false");
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}