using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Queries.GetOrganisations;

public sealed class GetOrganisationsQueryHandler : IHandler<NoopQuery, Result<IEnumerable<Organisation>>>
{
    private readonly IApplicationRepository _repository;

    public GetOrganisationsQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<IEnumerable<Organisation>>> HandleAsync(
        NoopQuery request, 
        CancellationToken cancellationToken)
    {
        return await _repository.Organisations
            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .Include(o => o.Locations)
            .ToListAsync(cancellationToken);
    }
}