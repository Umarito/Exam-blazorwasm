using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InstallmentOptionConfiguration : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("Installments");
        builder.HasKey(io => io.Id);
        builder.Property(io => io.MonthCount).IsRequired();
        builder.Property(io => io.InterestRate).HasPrecision(5, 2);
    }
}