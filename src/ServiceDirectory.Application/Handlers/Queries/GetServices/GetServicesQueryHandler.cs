using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Queries.GetServices;

public sealed class GetServicesQueryHandler : IHandler<NoopQuery, Result<IEnumerable<Service>>>
{
    private readonly IApplicationRepository _repository;

    public GetServicesQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<IEnumerable<Service>>> HandleAsync(NoopQuery request, CancellationToken cancellationToken)
    {
        var services = _repository.Organisations.SelectMany(o => o.Services).Include(s => s.Locations).ToList();
        return await Task.FromResult<Result<IEnumerable<Service>>>(services);
    }
}