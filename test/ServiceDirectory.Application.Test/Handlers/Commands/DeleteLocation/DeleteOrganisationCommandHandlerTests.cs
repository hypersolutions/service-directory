using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.DeleteLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.DeleteLocation;

public class DeleteLocationCommandHandlerTests : TestBase<DeleteLocationCommandHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly Location _discoveryHome;
    
    public DeleteLocationCommandHandlerTests()
    {
        _discoveryHome = TestLocations.DiscoveryHome();
        _repository = MockFor<IApplicationRepository>();
        _repository.Locations.Returns(new List<Location>([_discoveryHome]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ReturnsError()
    {
        var command = new DeleteLocationCommand(100);
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_UpdatesLocationStatusInactive()
    {
        var command = new DeleteLocationCommand(_discoveryHome.Id);
        
        await Subject.HandleAsync(command, CancellationToken.None);

        _discoveryHome.Status.ShouldBe(StatusType.Inactive);
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_SavesRepositoryChanges()
    {
        var command = new DeleteLocationCommand(_discoveryHome.Id);
        
        await Subject.HandleAsync(command, CancellationToken.None);

        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
}