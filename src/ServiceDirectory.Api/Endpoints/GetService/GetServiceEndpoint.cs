using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries.GetService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Api.Endpoints.GetService;

public class GetServiceEndpoint : EndpointBase<GetServiceRequest, ServiceDto>
{
    private readonly IHandler<GetServiceQuery, Result<Service>> _handler;

    public GetServiceEndpoint(IHandler<GetServiceQuery, Result<Service>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/service/{Id}");
        SetAdminPolicy();
        PreProcessor<RequestLoggerPreProcessor<GetServiceRequest>>();
    }

    public override async Task HandleAsync(GetServiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(new GetServiceQuery(new ServiceId(request.Id)), cancellationToken);
        
        await result.Match(
            s => SendOkAsync(ServiceDto.FromDomainModel(s), cancellationToken),
            e => SendHandlerErrorAsync(e, cancellationToken));
    }
}