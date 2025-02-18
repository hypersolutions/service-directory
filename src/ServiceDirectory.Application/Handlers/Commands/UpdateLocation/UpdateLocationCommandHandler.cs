using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateLocation;

public sealed class UpdateLocationCommandHandler : IHandler<UpdateLocationCommand, Result<Location>>
{
    private readonly IPostcodeClient _postcodeClient;
    private readonly IApplicationRepository _repository;

    public UpdateLocationCommandHandler(IPostcodeClient postcodeClient, IApplicationRepository repository)
    {
        _postcodeClient = postcodeClient;
        _repository = repository;
    }
    
    public async Task<Result<Location>> HandleAsync(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _postcodeClient.ResolvePostcodeLocationAsync(request.Postcode, cancellationToken);

        if (result.IsError) return (Error)result!;

        var postcodeCoordinate = (Coordinate)result!;
        
        var location = await _repository.Locations.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (location is null) return new Error($"Unable to find the location for {request.Id}.", ErrorType.NotFound);
        
        location.AddressLine1 = request.AddressLine1;
        location.AddressLine2 = request.AddressLine2;
        location.TownOrCity = request.TownOrCity;
        location.County = request.County;
        location.Postcode = request.Postcode;
        location.Latitude = postcodeCoordinate.Latitude;
        location.Longitude = postcodeCoordinate.Longitude;
        
        await _repository.SaveChangesAsync(cancellationToken);
        
        return location;
    }
}