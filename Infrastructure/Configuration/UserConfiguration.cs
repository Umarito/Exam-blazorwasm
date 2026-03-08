using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(g => g.Id);
        builder.Property(a => a.IsActive).HasDefaultValueSql("true");
        builder.HasIndex(a => a.Email).IsUnique();
        builder.Property(a => a.FullName).HasMaxLength(150);
        builder.HasOne(a => a.Cart).WithOne(a => a.User).HasForeignKey<Cart>(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(a=>a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}