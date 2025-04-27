using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(a => a.PostCode)
            .HasMaxLength(32);
    }
}
