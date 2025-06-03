using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.UpdateService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.UpdateService;

public class UpdateServiceCommandHandlerTests
{
    private readonly UpdateServiceCommandHandler _handler;
    private readonly Service _discoveryHomeService;
    
    public UpdateServiceCommandHandlerTests()
    {
        _handler = TestSubject.Create<UpdateServiceCommandHandler>();
        _discoveryHomeService = TestServices.DiscoveryHome();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Services).Returns(new List<Service>([_discoveryHomeService]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task UnknownService_HandleAsync_ThrowsException()
    {
        var command = GetUnknownUpdateServiceCommand();

        var result = await _handler.HandleAsync(command, CancellationToken.None);
        
        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the service for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task ValidService_HandleAsync_UpdatesServiceDetails()
    {
        var command = GetUpdateServiceCommand();

        await _handler.HandleAsync(command, CancellationToken.None);
        
        _discoveryHomeService.Name.ShouldBe(command.Name);
        _discoveryHomeService.Description.ShouldBe(command.Description);
        _discoveryHomeService.Cost.ShouldBe(command.Cost);
    }
    
    [Fact]
    public async Task ValidService_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetUpdateServiceCommand();

        await _handler.HandleAsync(command, CancellationToken.None);
        
        _handler.VerifyAll();
    }
    
    private UpdateServiceCommand GetUpdateServiceCommand() 
        => new(_discoveryHomeService.Id, "Discovery Home", "Discovery Home", 20.00M);
    
    private static UpdateServiceCommand GetUnknownUpdateServiceCommand() 
        => new(100, "Discovery Home", "Discovery Home", 20.00M);
}