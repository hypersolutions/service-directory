using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.UpdateLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.UpdateLocation;

public class UpdateLocationEndpoint : EndpointBase<UpdateLocationRequest, LocationDto>
{
    private readonly IHandler<UpdateLocationCommand, Result<Location>> _handler;

    public UpdateLocationEndpoint(IHandler<UpdateLocationCommand, Result<Location>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Put("/api/location");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<UpdateLocationRequest>>();
    }

    public override async Task HandleAsync(UpdateLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(
            new UpdateLocationCommand(
                request.Id,
                request.AddressLine1,
                request.AddressLine2,
                request.TownOrCity,
                request.County,
                request.Postcode), 
            cancellationToken);
        
        await result.Match(
            s => SendAsync(LocationDto.FromDomainModel(s), StatusCodes.Status200OK, cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}