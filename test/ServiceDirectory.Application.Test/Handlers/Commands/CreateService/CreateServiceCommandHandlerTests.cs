using NSubstitute;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.CreateService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.CreateService;

public class CreateServiceCommandHandlerTests : TestBase<CreateServiceCommandHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly Organisation _towerHamlets;
    private readonly Service _discoveryHomeService;
    private readonly Location _discoveryHomeLocation;
    private readonly IPostcodeClient _postcodeClient;
    
    public CreateServiceCommandHandlerTests()
    {
        _towerHamlets = TestOrganisations.TowerHamlets();
        _discoveryHomeService = TestServices.DiscoveryHome();
        _discoveryHomeLocation = TestLocations.DiscoveryHome();
        _repository = MockFor<IApplicationRepository>();
        _repository.Organisations.Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
        _postcodeClient = MockFor<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetCreateServiceCommand(_towerHamlets.Id, 10.00M);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ThrowsException()
    {
        var command = GetCreateServiceCommand(2, _discoveryHomeService.Cost);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        var result = await Subject.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 2.", Type: ErrorType.NotFound });
    }
    
    [Theory]
    [InlineData(-1.00)]
    [InlineData(2000.01)]
    public async Task InvalidCost_HandleAsync_ThrowsException(decimal cost)
    {
        var command = GetCreateServiceCommand(_towerHamlets.Id, cost);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        var result = await Subject.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e.Description == $"Unable to create a cost for {cost}." && e.Type == ErrorType.Unexpected);
    }
    
    [Fact]
    public async Task ValidService_HandleAsync_AddsServiceToOrganisation()
    {
        var command = GetCreateServiceCommand(_towerHamlets.Id, _discoveryHomeService.Cost);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        await Subject.HandleAsync(command, CancellationToken.None);
        
        _towerHamlets.Services.ShouldContain(s => s.Name == _discoveryHomeService.Name);
    }
    
    [Fact]
    public async Task ValidService_HandleAsync_AddsLocationToService()
    {
        var command = GetCreateServiceCommand(_towerHamlets.Id, _discoveryHomeService.Cost);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(
            s => s.Locations.FirstOrDefault(l => l.AddressLine1 == _discoveryHomeLocation.AddressLine1) != null);
    }
    
    [Fact]
    public async Task ValidService_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetCreateServiceCommand(_towerHamlets.Id, _discoveryHomeService.Cost);
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        await Subject.HandleAsync(command, CancellationToken.None);
        
        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
    
    private CreateServiceCommand GetCreateServiceCommand(int orgId, decimal cost) => new(
        orgId, _discoveryHomeService.Name, _discoveryHomeService.Description, cost, 
        _discoveryHomeLocation.AddressLine1, _discoveryHomeLocation.AddressLine2, 
        _discoveryHomeLocation.TownOrCity, _discoveryHomeLocation.County, 
        _discoveryHomeLocation.Postcode);
}