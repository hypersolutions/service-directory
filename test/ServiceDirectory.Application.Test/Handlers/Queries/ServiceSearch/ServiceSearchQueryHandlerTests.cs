using RockHopper;
using RockHopper.Mocking;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.ServiceSearch;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Xunit;
// ReSharper disable MethodHasAsyncOverload - test non-async too

namespace ServiceDirectory.Application.Test.Handlers.Queries.ServiceSearch;

public class ServiceSearchQueryHandlerTests
{
    private readonly ServiceSearchQueryHandler _handler;
    private readonly Mock<IApplicationRepository> _repository;
    private readonly Mock<IPostcodeClient> _postcodeClient;

    public ServiceSearchQueryHandlerTests()
    {
        _handler = TestSubject.Create<ServiceSearchQueryHandler>();
        _repository = _handler.GetMock<IApplicationRepository>();
        _postcodeClient = _handler.GetMock<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var query = new ServiceSearchQuery("SO19 8SJ", 10, 10);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);

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
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(51.518578, 0.06895));
        _repository.GetProperty(r => r.Organisations)
            .Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);

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
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(51.518578, 0.06895));
        _repository.GetProperty(r => r.Organisations)
            .Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);
        
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
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(query.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(51.518578, 0.06895));
        _repository.GetProperty(r => r.Organisations)
            .Returns(new List<Organisation>([towerHamletsOrganisation, cityOfLondonOrganisation]).AsTestQueryable());
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == parentsCentreService.Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == southWestJohnSmithService.Id));
    }
}