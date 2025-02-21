using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.UpdateService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.UpdateService;

public class UpdateServiceEndpoint : EndpointBase<UpdateServiceRequest, ServiceDto>
{
    private readonly IHandler<UpdateServiceCommand, Result<Service>> _handler;

    public UpdateServiceEndpoint(IHandler<UpdateServiceCommand, Result<Service>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Put("/api/service");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<UpdateServiceRequest>>();
    }

    public override async Task HandleAsync(UpdateServiceRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(request.Id, request.Name, request.Description, request.Cost);
        var result = await _handler.HandleAsync(command, cancellationToken);
        
        await result.Match(
            s => SendAsync(ServiceDto.FromDomainModel(s), StatusCodes.Status200OK, cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}