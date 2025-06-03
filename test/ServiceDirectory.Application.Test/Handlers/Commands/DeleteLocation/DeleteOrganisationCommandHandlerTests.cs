using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.DeleteLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.DeleteLocation;

public class DeleteLocationCommandHandlerTests
{
    private readonly DeleteLocationCommandHandler _handler;
    private readonly Location _discoveryHome;
    
    public DeleteLocationCommandHandlerTests()
    {
        _handler = TestSubject.Create<DeleteLocationCommandHandler>();
        _discoveryHome = TestLocations.DiscoveryHome();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Locations).Returns(new List<Location>([_discoveryHome]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ReturnsError()
    {
        var command = new DeleteLocationCommand(100);
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_UpdatesLocationStatusInactive()
    {
        var command = new DeleteLocationCommand(_discoveryHome.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _discoveryHome.Status.ShouldBe(StatusType.Inactive);
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_SavesRepositoryChanges()
    {
        var command = new DeleteLocationCommand(_discoveryHome.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _handler.VerifyAll();
    }
}