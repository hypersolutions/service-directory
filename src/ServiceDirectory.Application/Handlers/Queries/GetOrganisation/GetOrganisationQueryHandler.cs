using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Queries.GetOrganisation;

public sealed class GetOrganisationQueryHandler : IHandler<GetOrganisationQuery, Result<Organisation>>
{
    private readonly IApplicationRepository _repository;

    public GetOrganisationQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Organisation>> HandleAsync(
        GetOrganisationQuery request, 
        CancellationToken cancellationToken)
    {
        var organisation = await _repository.Organisations
            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .Include(o => o.Locations)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (organisation is null) return new Error($"Unable to find the organisation for {request.Id}.", ErrorType.NotFound);
        
        return organisation;
    }
}