using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.CreateLocation;

public sealed class CreateLocationCommandHandler : IHandler<CreateLocationCommand, Result<Location>>
{
    private readonly IPostcodeClient _postcodeClient;
    private readonly IApplicationRepository _repository;

    public CreateLocationCommandHandler(IPostcodeClient postcodeClient, IApplicationRepository repository)
    {
        _postcodeClient = postcodeClient;
        _repository = repository;
    }
    
    public async Task<Result<Location>> HandleAsync(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _postcodeClient.ResolvePostcodeLocationAsync(request.Postcode, cancellationToken);

        if (result.IsError) return (Error)result!;

        var postcodeCoordinate = (Coordinate)result!;

        if (request.ServiceId is not null)
            return await HandleServiceAsync(request, postcodeCoordinate, cancellationToken);
        
        if (request.OrganisationId is not null)
            return await HandleOrganisationAsync(request, postcodeCoordinate, cancellationToken);

        return new Error("Unable to create a location for either a service or organisation.", ErrorType.Unexpected);
    }

    private async Task<Result<Location>> HandleServiceAsync(
        CreateLocationCommand request,
        Coordinate postcodeCoordinate,
        CancellationToken cancellationToken)
    {
        var service = await _repository.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);

        if (service is null) return new Error($"Unable to find the service for {request.ServiceId}.", ErrorType.NotFound);
        
        var location = CreateLocation(request, postcodeCoordinate);
        service.AddLocation(location);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return location;
    }

    private async Task<Result<Location>> HandleOrganisationAsync(
        CreateLocationCommand request,
        Coordinate postcodeCoordinate,
        CancellationToken cancellationToken)
    {
        var organisation = await _repository.Organisations.FirstOrDefaultAsync(s => s.Id == request.OrganisationId, cancellationToken);

        if (organisation is null) return new Error($"Unable to find the organisation for {request.OrganisationId}.", ErrorType.NotFound);

        var location = CreateLocation(request, postcodeCoordinate);
        organisation.AddLocation(location);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return location;
    }

    private static Location CreateLocation(CreateLocationCommand request, Coordinate postcodeCoordinate)
        => new()
        {
            AddressLine1 = request.AddressLine1, 
            AddressLine2 = request.AddressLine2, 
            TownOrCity = request.TownOrCity,
            County = request.County, 
            Postcode = request.Postcode, 
            Latitude = postcodeCoordinate.Latitude,
            Longitude = postcodeCoordinate.Longitude
        };
}