using RockHopper;
using RockHopper.Mocking;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.UpdateLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.UpdateLocation;

public class UpdateLocationCommandHandlerTests
{
    private readonly UpdateLocationCommandHandler _handler;
    private readonly Mock<IPostcodeClient> _postcodeClient;
    private readonly Location _discoveryHomeLocation;
    
    public UpdateLocationCommandHandlerTests()
    {
        _handler = TestSubject.Create<UpdateLocationCommandHandler>();
        _discoveryHomeLocation = TestLocations.DiscoveryHome();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Locations).Returns(new List<Location>([_discoveryHomeLocation]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        _postcodeClient = _handler.GetMock<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetUpdateLocationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ThrowsException()
    {
        var command = GetUnknownUpdateLocationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task ValidLocation_HandleAsync_UpdatesLocationDetails()
    {
        var command = GetUpdateLocationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _discoveryHomeLocation.AddressLine1.ShouldBe(command.AddressLine1);
        _discoveryHomeLocation.AddressLine2.ShouldBe(command.AddressLine2);
        _discoveryHomeLocation.TownOrCity.ShouldBe(command.TownOrCity);
        _discoveryHomeLocation.County.ShouldBe(command.County);
        _discoveryHomeLocation.Postcode.ShouldBe(command.Postcode);
    }
    
    [Fact]
    public async Task ValidLocation_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetUpdateLocationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        
        await _handler.HandleAsync(command, CancellationToken.None);
        
        _handler.VerifyAll();
    }
    
    private UpdateLocationCommand GetUpdateLocationCommand() => new(
        _discoveryHomeLocation.Id, "The Quab Break Service", "31 - 35 Spelman Street", 
        "London", "London", "E1 5LQ");
    
    private UpdateLocationCommand GetUnknownUpdateLocationCommand() => new(
        100, "The Quab Break Service", "31 - 35 Spelman Street", 
        "London", "London", "E1 5LQ");
}