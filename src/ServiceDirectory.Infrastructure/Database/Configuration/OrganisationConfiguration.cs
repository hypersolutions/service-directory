using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Infrastructure.Database.Configuration;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().HasConversion(p => (int)p, p => p);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.Status).IsRequired().HasConversion(p => p.ToString(), p => Enum.Parse<StatusType>(p));
        builder.HasMany(p => p.Services);
        builder.HasMany(p => p.Locations);
        builder.HasQueryFilter(p => p.Status == StatusType.Active);
    }
}