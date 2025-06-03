using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.DeleteOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.DeleteOrganisation;

public class DeleteOrganisationCommandHandlerTests
{
    private readonly DeleteOrganisationCommandHandler _handler;
    private readonly Organisation _towerHamlets;
    
    public DeleteOrganisationCommandHandlerTests()
    {
        _handler = TestSubject.Create<DeleteOrganisationCommandHandler>();
        _towerHamlets = TestOrganisations.TowerHamlets();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Organisations).Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ReturnsError()
    {
        var command = new DeleteOrganisationCommand(100);
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownOrganisation_HandleAsync_UpdatesOrganisationStatusInactive()
    {
        var command = new DeleteOrganisationCommand(_towerHamlets.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _towerHamlets.Status.ShouldBe(StatusType.Inactive);
    }
    
    [Fact]
    public async Task KnownOrganisation_HandleAsync_SavesRepositoryChanges()
    {
        var command = new DeleteOrganisationCommand(_towerHamlets.Id);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _handler.VerifyAll();
    }
}