using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.CreateService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.CreateService;

public class CreateServiceEndpoint : EndpointBase<CreateServiceRequest, ServiceDto>
{
    private readonly IHandler<CreateServiceCommand, Result<Service>> _handler;

    public CreateServiceEndpoint(IHandler<CreateServiceCommand, Result<Service>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Post("/api/service");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<CreateServiceRequest>>();
    }

    public override async Task HandleAsync(CreateServiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(
            new CreateServiceCommand(
                request.OrganisationId,
                request.Name, 
                request.Description,
                request.Cost,
                request.AddressLine1,
                request.AddressLine2,
                request.TownOrCity,
                request.County,
                request.Postcode), cancellationToken);
        
        await result.Match(
            s => SendCreatedAsync(ServiceDto.FromDomainModel(s), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}