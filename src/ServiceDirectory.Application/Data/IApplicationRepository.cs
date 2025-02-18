using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Data;

public interface IApplicationRepository
{
    IQueryable<Organisation> Organisations { get; }
    IQueryable<Service> Services { get; }
    IQueryable<Location> Locations { get; }

    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}