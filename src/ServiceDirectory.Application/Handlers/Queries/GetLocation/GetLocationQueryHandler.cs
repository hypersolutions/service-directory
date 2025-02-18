using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Queries.GetLocation;

public sealed class GetLocationQueryHandler : IHandler<GetLocationQuery, Result<Location>>
{
    private readonly IApplicationRepository _repository;

    public GetLocationQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Location>> HandleAsync(
        GetLocationQuery request, 
        CancellationToken cancellationToken)
    {
        var location = await _repository.Locations.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
        
        if (location is null) 
            return new Error($"Unable to find the location for {request.Id}.", ErrorType.NotFound);
        
        return location;
    }
}