using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StoreLocationConfiguration : IEntityTypeConfiguration<StoreLocation>
{
    public void Configure(EntityTypeBuilder<StoreLocation> builder)
    {
        builder.ToTable("StoreLocations");
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Address).IsRequired().HasMaxLength(250);
        builder.Property(sl => sl.WorkingHours).HasMaxLength(100);
    }
}