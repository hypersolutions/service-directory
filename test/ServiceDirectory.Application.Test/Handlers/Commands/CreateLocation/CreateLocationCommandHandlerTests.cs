using RockHopper;
using RockHopper.Mocking;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.CreateLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.CreateLocation;

public class CreateLocationCommandHandlerTests
{
    private readonly CreateLocationCommandHandler _handler;
    private readonly Mock<IApplicationRepository> _repository;
    private readonly Mock<IPostcodeClient> _postcodeClient;
    private readonly Organisation _towerHamlets;
    private readonly Service _discoveryHomeService;
    private readonly Location _discoveryHomeLocation;
    
    public CreateLocationCommandHandlerTests()
    {
        _towerHamlets = TestOrganisations.TowerHamlets();
        _discoveryHomeService = TestServices.DiscoveryHome();
        _discoveryHomeLocation = TestLocations.DiscoveryHome();
        _handler = TestSubject.Create<CreateLocationCommandHandler>();
        _repository = _handler.GetMock<IApplicationRepository>();
        _postcodeClient = _handler.GetMock<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetCreateServiceLocationCommand(_discoveryHomeService.Id);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task UnknownService_HandleAsync_ThrowsException()
    {
        var command = GetCreateServiceLocationCommand(100);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.GetProperty(r => r.Services).Returns(new List<Service>([_discoveryHomeService]).AsTestQueryable());
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the service for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task ValidLocationForService_HandleAsync_AddsLocationToService()
    {
        var command = GetCreateServiceLocationCommand(_discoveryHomeService.Id);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.GetProperty(r => r.Services).Returns(new List<Service>([_discoveryHomeService]).AsTestQueryable());
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        
        await _handler.HandleAsync(command, CancellationToken.None);
        
        _discoveryHomeService.Locations.ShouldContain(
            l => l.AddressLine1 == _discoveryHomeLocation.AddressLine1 &&
                 l.AddressLine2 == _discoveryHomeLocation.AddressLine2 &&
                 l.TownOrCity == _discoveryHomeLocation.TownOrCity &&
                 l.County == _discoveryHomeLocation.County &&
                 l.Postcode == _discoveryHomeLocation.Postcode);
    }
    
    [Fact]
    public async Task ValidLocationForService_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetCreateServiceLocationCommand(_discoveryHomeService.Id);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.GetProperty(r => r.Services).Returns(new List<Service>([_discoveryHomeService]).AsTestQueryable());
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        
        await _handler.HandleAsync(command, CancellationToken.None);
        
        _handler.VerifyAll();
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ThrowsException()
    {
        var command = GetCreateOrganisationLocationCommand(100);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.GetProperty(r => r.Organisations).Returns(new TestAsyncEnumerable<Organisation>([_towerHamlets]));
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task ValidLocationForOrganisation_HandleAsync_AddsLocationToOrganisation()
    {
        var command = GetCreateOrganisationLocationCommand(_towerHamlets.Id);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        _repository.GetProperty(r => r.Organisations).Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
        
        await _handler.HandleAsync(command, CancellationToken.None);
        
        _towerHamlets.Locations.ShouldContain(
            l => l.AddressLine1 == _discoveryHomeLocation.AddressLine1 &&
                 l.AddressLine2 == _discoveryHomeLocation.AddressLine2 &&
                 l.TownOrCity == _discoveryHomeLocation.TownOrCity &&
                 l.County == _discoveryHomeLocation.County &&
                 l.Postcode == _discoveryHomeLocation.Postcode);
        
        _handler.VerifyAll();
    }
    
    [Fact]
    public async Task ValidLocationForOrganisation_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetCreateOrganisationLocationCommand(_towerHamlets.Id);
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.GetProperty(r => r.Organisations).Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);

        await _handler.HandleAsync(command, CancellationToken.None);
        
        _handler.VerifyAll();
    }
    
    private CreateLocationCommand GetCreateServiceLocationCommand(int? serviceId) => new(
        serviceId, null, _discoveryHomeLocation.AddressLine1, _discoveryHomeLocation.AddressLine2, 
        _discoveryHomeLocation.TownOrCity, _discoveryHomeLocation.County, _discoveryHomeLocation.Postcode);
    
    private CreateLocationCommand GetCreateOrganisationLocationCommand(int? organisationId) => new(
        null, organisationId, _discoveryHomeLocation.AddressLine1, _discoveryHomeLocation.AddressLine2, 
        _discoveryHomeLocation.TownOrCity, _discoveryHomeLocation.County, _discoveryHomeLocation.Postcode);
}