using NSubstitute;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.ServiceSearch;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Queries.ServiceSearch;

public class ServiceSearchQueryHandlerTests : TestBase<ServiceSearchQueryHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly IPostcodeClient _postcodeClient;

    public ServiceSearchQueryHandlerTests()
    {
        _repository = MockFor<IApplicationRepository>();
        _postcodeClient = MockFor<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var query = new ServiceSearchQuery("SO19 8SJ", 10, 10);
        _postcodeClient.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None).Returns(new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task PostcodeExists_HandleAsync_ReturnsServicesWithin5Kms()
    {
        var parentsCentreService = TestServices.ParentsCentre();
        var southWestJohnSmithService = TestServices.SouthWestJohnSmith();
        var towerHamletsOrganisation = TestOrganisations.TowerHamlets();
        var cityOfLondonOrganisation = TestOrganisations.CityOfLondon();
        
        var query = new ServiceSearchQuery("E1 5LQ", 5, 5);
        _postcodeClient.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None).Returns(new Coordinate(51.518578, 0.06895));
        _repository.Organisations.Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == parentsCentreService.Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == southWestJohnSmithService.Id));
    }
    
    [Fact]
    public async Task PostcodeExists_HandleAsync_ReturnsServicesWithin10Kms()
    {
        var parentsCentreService = TestServices.ParentsCentre();
        var specialistMidwifeService = TestServices.SpecialistMidwife();
        var southWestJohnSmithService = TestServices.SouthWestJohnSmith();
        var towerHamletsOrganisation = TestOrganisations.TowerHamlets();
        var cityOfLondonOrganisation = TestOrganisations.CityOfLondon();
        
        var query = new ServiceSearchQuery("E1 5LQ", 10, 5);
        _postcodeClient.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None).Returns(new Coordinate(51.518578, 0.06895));
        _repository.Organisations.Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);
        
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == parentsCentreService.Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == specialistMidwifeService.Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == southWestJohnSmithService.Id));
    }
    
    [Fact]
    public async Task PostcodeExists_HandleAsync_ReturnsServicesWithin10KmsLimitedTo2()
    {
        var parentsCentreService = TestServices.ParentsCentre();
        var southWestJohnSmithService = TestServices.SouthWestJohnSmith();
        var towerHamletsOrganisation = TestOrganisations.TowerHamlets();
        var cityOfLondonOrganisation = TestOrganisations.CityOfLondon();
        
        var query = new ServiceSearchQuery("E1 5LQ", 10, 2);
        _postcodeClient.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None).Returns(new Coordinate(51.518578, 0.06895));
        _repository.Organisations.Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == parentsCentreService.Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == southWestJohnSmithService.Id));
    }
}