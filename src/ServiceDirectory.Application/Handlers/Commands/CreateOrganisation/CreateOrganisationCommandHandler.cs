using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;

public sealed class CreateOrganisationCommandHandler : IHandler<CreateOrganisationCommand, Result<Organisation>>
{
    private readonly IPostcodeClient _postcodeClient;
    private readonly IApplicationRepository _repository;

    public CreateOrganisationCommandHandler(IPostcodeClient postcodeClient, IApplicationRepository repository)
    {
        _postcodeClient = postcodeClient;
        _repository = repository;
    }
    
    public async Task<Result<Organisation>> HandleAsync(
        CreateOrganisationCommand request, 
        CancellationToken cancellationToken)
    {
        var result = await _postcodeClient.ResolvePostcodeLocationAsync(request.Postcode, cancellationToken);

        if (result.IsError) return (Error)result!;

        var postcodeCoordinate = (Coordinate)result!;
        var organisation = new Organisation { Name = request.Name, Description = request.Description };
        var location = CreateLocation(request, postcodeCoordinate);
        organisation.AddLocation(location);

        await _repository.AddAsync(organisation, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return organisation;
    }
    
    private static Location CreateLocation(CreateOrganisationCommand request, Coordinate postcodeCoordinate)
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