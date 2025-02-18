using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.UpdateOrganisation;

public class UpdateOrganisationEndpoint : EndpointBase<UpdateOrganisationRequest, OrganisationDto>
{
    private readonly IHandler<UpdateOrganisationCommand, Result<Organisation>> _handler;

    public UpdateOrganisationEndpoint(IHandler<UpdateOrganisationCommand, Result<Organisation>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Put("/api/organisation");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<UpdateOrganisationRequest>>();
    }

    public override async Task HandleAsync(UpdateOrganisationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(
            new UpdateOrganisationCommand(request.Id, request.Name, request.Description), cancellationToken);
        
        await result.Match(
            s => SendAsync(OrganisationDto.FromDomainModel(s), StatusCodes.Status200OK, cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}