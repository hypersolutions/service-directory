using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Infrastructure.Database.Configuration;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().HasConversion(p => (int)p, p => p);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Ignore(p => p.Cost);
        builder.Property(p => p.Cost).HasConversion(v => (decimal)v, v => v);
        builder.HasMany(p => p.Locations);
        builder.Property(p => p.Status).IsRequired().HasConversion(p => p.ToString(), p => Enum.Parse<StatusType>(p));
        builder.HasQueryFilter(p => p.Status == StatusType.Active);
    }
}