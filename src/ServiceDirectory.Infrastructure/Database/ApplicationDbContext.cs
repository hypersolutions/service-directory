using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Organisation> Organisations { get; set; }
    
    public DbSet<Service> Services { get; set; }
    
    public DbSet<Location> Locations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}