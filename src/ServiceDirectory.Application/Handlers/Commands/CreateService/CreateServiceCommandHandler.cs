using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.CreateService;

public sealed class CreateServiceCommandHandler : IHandler<CreateServiceCommand, Result<Service>>
{
    private readonly IApplicationRepository _repository;
    private readonly IPostcodeClient _postcodeClient;

    public CreateServiceCommandHandler(IPostcodeClient postcodeClient, IApplicationRepository repository)
    {
        _postcodeClient = postcodeClient;
        _repository = repository;
    }
    
    public async Task<Result<Service>> HandleAsync(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var result = await _postcodeClient.ResolvePostcodeLocationAsync(request.Postcode, cancellationToken);

        if (result.IsError) return (Error)result!;
        
        var postcodeCoordinate = (Coordinate)result!;
        var organisation = await _repository.Organisations.FirstOrDefaultAsync(o => o.Id == request.OrganisationId, cancellationToken);

        if (organisation is null) return UnknownOrganisationError(request.OrganisationId);
        
        if (!Cost.TryCreate(request.Cost, out var cost)) return InvalidServiceCostError(request.Cost);

        var service = new Service { Name = request.Name, Description = request.Description, Cost = cost };
        var location = CreateLocation(request, postcodeCoordinate);
        service.AddLocation(location);
        organisation.AddService(service);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return service;
    }

    private static Error UnknownOrganisationError(int orgId) 
        => new($"Unable to find the organisation for {orgId}.", ErrorType.NotFound);

    private static Error InvalidServiceCostError(decimal cost) =>
        new($"Unable to create a cost for {cost}.", ErrorType.Unexpected);
    
    private static Location CreateLocation(CreateServiceCommand request, Coordinate postcodeCoordinate)
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