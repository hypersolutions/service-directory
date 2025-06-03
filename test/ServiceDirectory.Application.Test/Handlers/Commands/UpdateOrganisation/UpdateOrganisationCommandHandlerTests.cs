using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.UpdateOrganisation;

public class UpdateOrganisationCommandHandlerTests
{
    private readonly UpdateOrganisationCommandHandler _handler;
    private readonly Organisation _towerHamlets;
    
    public UpdateOrganisationCommandHandlerTests()
    {
        _handler = TestSubject.Create<UpdateOrganisationCommandHandler>();
        _towerHamlets = TestOrganisations.TowerHamlets();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Organisations).Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
        repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ReturnsError()
    {
        var command = new UpdateOrganisationCommand(100, "Tower Hamlets", "Updated Tower Hamlets description");
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownOrganisation_HandleAsync_UpdatesOrganisationDetails()
    {
        var command = new UpdateOrganisationCommand(_towerHamlets.Id, "Tower Hamlets", "Updated Tower Hamlets description");
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _towerHamlets.Name.ShouldBe(command.Name);
        _towerHamlets.Description.ShouldBe(command.Description);
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_SavesRepositoryChanges()
    {
        var command = new UpdateOrganisationCommand(_towerHamlets.Id, "Tower Hamlets", "Updated Tower Hamlets description");
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _handler.VerifyAll();
    }
}