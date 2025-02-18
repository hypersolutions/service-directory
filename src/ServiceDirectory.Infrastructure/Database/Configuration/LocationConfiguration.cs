using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Infrastructure.Database.Configuration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().HasConversion(p => (int)p, p => p);
        builder.Property(p => p.AddressLine1).IsRequired();
        builder.Property(p => p.TownOrCity).IsRequired();
        builder.Property(p => p.County).IsRequired();
        builder.Property(p => p.Postcode).IsRequired();
        builder.Property(p => p.Latitude).HasConversion(v => (double)v, v => v);
        builder.Property(p => p.Longitude).HasConversion(v => (double)v, v => v);
        builder.Property(p => p.Status).IsRequired().HasConversion(p => p.ToString(), p => Enum.Parse<StatusType>(p));
        builder.HasQueryFilter(p => p.Status == StatusType.Active);
    }
}