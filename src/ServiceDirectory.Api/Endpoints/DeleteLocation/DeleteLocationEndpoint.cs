using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.DeleteLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.DeleteLocation;

public class DeleteLocationEndpoint : EndpointBase<DeleteLocationRequest>
{
    private readonly IHandler<DeleteLocationCommand, Result<Location>> _handler;

    public DeleteLocationEndpoint(IHandler<DeleteLocationCommand, Result<Location>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Delete("/api/location/{Id}");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<DeleteLocationRequest>>();
    }

    public override async Task HandleAsync(DeleteLocationRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteLocationCommand(request.Id);
        var result = await _handler.HandleAsync(command, cancellationToken);
        
        await result.Match(
            _ => SendOkAsync(cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}