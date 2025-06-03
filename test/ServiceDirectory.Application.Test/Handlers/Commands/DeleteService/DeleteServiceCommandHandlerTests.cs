using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.DeleteService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.DeleteService;

public class DeleteServiceCommandHandlerTests
{
    private readonly DeleteServiceCommandHandler _handler;
    private readonly Service _discoveryHome;
    
    public DeleteServiceCommandHandlerTests()
    {
        _handler = TestSubject.Create<DeleteServiceCommandHandler>();
        _discoveryHome = TestServices.DiscoveryHome();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Services).Returns(new List<Service>([_discoveryHome]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task UnknownService_HandleAsync_ReturnsError()
    {
        var command = new DeleteServiceCommand(100);
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the service for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownService_HandleAsync_UpdatesServiceStatusInactive()
    {
        var command = new DeleteServiceCommand(_discoveryHome.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _discoveryHome.Status.ShouldBe(StatusType.Inactive);
    }
    
    [Fact]
    public async Task KnownService_HandleAsync_SavesRepositoryChanges()
    {
        var command = new DeleteServiceCommand(_discoveryHome.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _handler.VerifyAll();
    }
}