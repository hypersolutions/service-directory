using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.GetServices;

public class GetServicesEndpoint : EndpointBase
{
    private readonly IHandler<NoopQuery, Result<IEnumerable<Service>>> _handler;

    public GetServicesEndpoint(IHandler<NoopQuery, Result<IEnumerable<Service>>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/service");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(new NoopQuery(), cancellationToken);
        await result.Match(
            s => SendOkAsync(s.Select(ServiceDto.FromDomainModel), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}