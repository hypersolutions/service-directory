using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries.GetOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Api.Endpoints.GetOrganisation;

public class GetOrganisationEndpoint : EndpointBase<GetOrganisationRequest, OrganisationDto>
{
    private readonly IHandler<GetOrganisationQuery, Result<Organisation>> _handler;

    public GetOrganisationEndpoint(IHandler<GetOrganisationQuery, Result<Organisation>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/organisation/{Id}");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<GetOrganisationRequest>>();
    }

    public override async Task HandleAsync(GetOrganisationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(new GetOrganisationQuery(new OrganisationId(request.Id)), cancellationToken);
        
        await result.Match(
            s => SendOkAsync(OrganisationDto.FromDomainModel(s), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}