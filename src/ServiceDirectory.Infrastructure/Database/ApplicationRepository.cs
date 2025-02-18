using ServiceDirectory.Application.Data;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Infrastructure.Database;

public sealed class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Organisation> Organisations => _context.Organisations;
    
    public IQueryable<Service> Services => _context.Services;
    
    public IQueryable<Location> Locations => _context.Locations;

    public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
    {
        await _context.AddAsync(entity, cancellationToken);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}