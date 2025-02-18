using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.CreateOrganisation;

public class CreateOrganisationEndpoint : EndpointBase<CreateOrganisationRequest, OrganisationDto>
{
    private readonly IHandler<CreateOrganisationCommand, Result<Organisation>> _handler;

    public CreateOrganisationEndpoint(IHandler<CreateOrganisationCommand, Result<Organisation>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Post("/api/organisation");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<CreateOrganisationRequest>>();
    }

    public override async Task HandleAsync(CreateOrganisationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(
            new CreateOrganisationCommand(
                request.Name, 
                request.Description,
                request.AddressLine1,
                request.AddressLine2,
                request.TownOrCity,
                request.County,
                request.Postcode), 
            cancellationToken);
        
        await result.Match(
            s => SendCreatedAsync(OrganisationDto.FromDomainModel(s), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}