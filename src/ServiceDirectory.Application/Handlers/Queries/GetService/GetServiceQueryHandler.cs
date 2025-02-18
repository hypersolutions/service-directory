using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Queries.GetService;

public sealed class GetServiceQueryHandler : IHandler<GetServiceQuery, Result<Service>>
{
    private readonly IApplicationRepository _repository;

    public GetServiceQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Service>> HandleAsync(
        GetServiceQuery request, 
        CancellationToken cancellationToken)
    {
        var service = await _repository.Services.Include(s => s.Locations).FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (service is null) 
            return new Error($"Unable to find the service for {request.Id}.", ErrorType.NotFound);
        
        return service;
    }
}