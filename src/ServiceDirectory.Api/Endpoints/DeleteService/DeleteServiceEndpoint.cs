using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.DeleteService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.DeleteService;

public class DeleteServiceEndpoint : EndpointBase<DeleteServiceRequest>
{
    private readonly IHandler<DeleteServiceCommand, Result<Service>> _handler;

    public DeleteServiceEndpoint(IHandler<DeleteServiceCommand, Result<Service>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Delete("/api/service/{Id}");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<DeleteServiceRequest>>();
    }

    public override async Task HandleAsync(DeleteServiceRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteServiceCommand(request.Id);
        var result = await _handler.HandleAsync(command, cancellationToken);
        
        await result.Match(
            _ => SendOkAsync(cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}