using NSubstitute;
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

public class UpdateLocationCommandHandlerTests : TestBase<UpdateLocationCommandHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly IPostcodeClient _postcodeClient;
    private readonly Location _discoveryHomeLocation;
    
    public UpdateLocationCommandHandlerTests()
    {
        _discoveryHomeLocation = TestLocations.DiscoveryHome();
        _repository = MockFor<IApplicationRepository>();
        _repository.Locations.Returns(new List<Location>([_discoveryHomeLocation]).AsTestQueryable());
        _postcodeClient = MockFor<IPostcodeClient>();
    }
    
    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetUpdateLocationCommand();
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ThrowsException()
    {
        var command = GetUnknownUpdateLocationCommand();

        var result = await Subject.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task ValidLocation_HandleAsync_UpdatesLocationDetails()
    {
        var command = GetUpdateLocationCommand();
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        await Subject.HandleAsync(command, CancellationToken.None);

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
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));

        await Subject.HandleAsync(command, CancellationToken.None);
        
        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
    
    private UpdateLocationCommand GetUpdateLocationCommand() => new(
        _discoveryHomeLocation.Id, "The Quab Break Service", "31 - 35 Spelman Street", 
        "London", "London", "E1 5LQ");
    
    private UpdateLocationCommand GetUnknownUpdateLocationCommand() => new(
        100, "The Quab Break Service", "31 - 35 Spelman Street", 
        "London", "London", "E1 5LQ");
}