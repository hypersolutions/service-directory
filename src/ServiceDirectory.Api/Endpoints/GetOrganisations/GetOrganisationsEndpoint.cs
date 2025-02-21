using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.GetOrganisations;

public class GetOrganisationsEndpoint : EndpointBase
{
    private readonly IHandler<NoopQuery, Result<IEnumerable<Organisation>>> _handler;

    public GetOrganisationsEndpoint(IHandler<NoopQuery, Result<IEnumerable<Organisation>>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/organisation");
        SetAdminPolicy();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(new NoopQuery(), cancellationToken);
        await result.Match(
            s => SendOkAsync(s.Select(OrganisationDto.FromDomainModel), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}