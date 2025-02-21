using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.CreateLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.CreateLocation;

public class CreateLocationEndpoint : EndpointBase<CreateLocationRequest, LocationDto>
{
    private readonly IHandler<CreateLocationCommand, Result<Location>> _handler;

    public CreateLocationEndpoint(IHandler<CreateLocationCommand, Result<Location>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Post("/api/location");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<CreateLocationRequest>>();
    }

    public override async Task HandleAsync(CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(
            new CreateLocationCommand(
                request.ServiceId,
                request.OrganisationId,
                request.AddressLine1,
                request.AddressLine2,
                request.TownOrCity,
                request.County,
                request.Postcode), 
            cancellationToken);
        
        await result.Match(
            s => SendCreatedAsync(LocationDto.FromDomainModel(s), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}