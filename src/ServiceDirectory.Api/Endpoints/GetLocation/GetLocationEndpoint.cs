using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Queries.GetLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Api.Endpoints.GetLocation;

public class GetLocationEndpoint : EndpointBase<GetLocationRequest, LocationDto>
{
    private readonly IHandler<GetLocationQuery, Result<Location>> _handler;

    public GetLocationEndpoint(IHandler<GetLocationQuery, Result<Location>> handler)
    {
        _handler = handler;
    }

    public override void Configure()
    {
        Get("/api/location/{Id}");
        AllowAnonymous();
        PreProcessor<RequestLoggerPreProcessor<GetLocationRequest>>();
    }

    public override async Task HandleAsync(GetLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(new GetLocationQuery(new LocationId(request.Id)), cancellationToken);
        
        await result.Match(
            l => SendOkAsync(LocationDto.FromDomainModel(l), cancellationToken),
            _ => SendNoContentAsync(cancellationToken));
    }
}