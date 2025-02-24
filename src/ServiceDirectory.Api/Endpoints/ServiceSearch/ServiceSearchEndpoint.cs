using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries.ServiceSearch;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Endpoints.ServiceSearch;

public class ServiceSearchEndpoint : EndpointBase<ServiceSearchRequest, IEnumerable<ServiceDetailDto>>
{
    private readonly IHandler<ServiceSearchQuery, Result<IEnumerable<ServiceDetail>>> _handler;

    public ServiceSearchEndpoint(IHandler<ServiceSearchQuery, Result<IEnumerable<ServiceDetail>>> handler)
    {
        _handler = handler;
    }
    
    public override void Configure()
    {
        Get("/api/service/search");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<ServiceSearchRequest>>();
    }

    public override async Task HandleAsync(ServiceSearchRequest request, CancellationToken cancellationToken)
    {
        var query = new ServiceSearchQuery(request.Postcode, request.Distance, request.MaxResults);
        var result = await _handler.HandleAsync(query, cancellationToken);
        await result.Match(
           s => SendOkAsync(s.Select(ServiceDetailDto.FromDomainModel), cancellationToken), 
           e => SendHandlerErrorAsync(e, cancellationToken));
    }
}