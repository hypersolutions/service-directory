using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.DeleteOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.DeleteOrganisation;

public class DeleteOrganisationEndpoint : EndpointBase<DeleteOrganisationRequest>
{
    private readonly IHandler<DeleteOrganisationCommand, Result<Organisation>> _handler;

    public DeleteOrganisationEndpoint(IHandler<DeleteOrganisationCommand, Result<Organisation>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Delete("/api/organisation/{Id}");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<DeleteOrganisationRequest>>();
    }

    public override async Task HandleAsync(DeleteOrganisationRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteOrganisationCommand(request.Id);
        var result = await _handler.HandleAsync(command, cancellationToken);
        
        await result.Match(
            _ => SendOkAsync(cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}