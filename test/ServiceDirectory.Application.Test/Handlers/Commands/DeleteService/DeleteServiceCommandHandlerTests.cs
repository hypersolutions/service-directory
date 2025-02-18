using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.DeleteService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.DeleteService;

public class DeleteServiceCommandHandlerTests : TestBase<DeleteServiceCommandHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly Service _discoveryHome;
    
    public DeleteServiceCommandHandlerTests()
    {
        _discoveryHome = TestServices.DiscoveryHome();
        _repository = MockFor<IApplicationRepository>();
        _repository.Services.Returns(new List<Service>([_discoveryHome]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownService_HandleAsync_ReturnsError()
    {
        var command = new DeleteServiceCommand(100);
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the service for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownService_HandleAsync_UpdatesServiceStatusInactive()
    {
        var command = new DeleteServiceCommand(_discoveryHome.Id);
        
        await Subject.HandleAsync(command, CancellationToken.None);

        _discoveryHome.Status.ShouldBe(StatusType.Inactive);
    }
    
    [Fact]
    public async Task KnownService_HandleAsync_SavesRepositoryChanges()
    {
        var command = new DeleteServiceCommand(_discoveryHome.Id);
        
        await Subject.HandleAsync(command, CancellationToken.None);

        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
}