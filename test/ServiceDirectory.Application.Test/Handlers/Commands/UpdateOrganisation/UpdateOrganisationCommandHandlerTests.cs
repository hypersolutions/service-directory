using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.UpdateOrganisation;

public class UpdateOrganisationCommandHandlerTests : TestBase<UpdateOrganisationCommandHandler>
{
    private readonly IApplicationRepository _repository;
    private readonly Organisation _towerHamlets;
    
    public UpdateOrganisationCommandHandlerTests()
    {
        _towerHamlets = TestOrganisations.TowerHamlets();
        _repository = MockFor<IApplicationRepository>();
        _repository.Organisations.Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ReturnsError()
    {
        var command = new UpdateOrganisationCommand(100, "Tower Hamlets", "Updated Tower Hamlets description");
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownOrganisation_HandleAsync_UpdatesOrganisationDetails()
    {
        var command = new UpdateOrganisationCommand(_towerHamlets.Id, "Tower Hamlets", "Updated Tower Hamlets description");
        
        await Subject.HandleAsync(command, CancellationToken.None);

        _towerHamlets.Name.ShouldBe(command.Name);
        _towerHamlets.Description.ShouldBe(command.Description);
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_SavesRepositoryChanges()
    {
        var command = new UpdateOrganisationCommand(_towerHamlets.Id, "Tower Hamlets", "Updated Tower Hamlets description");
        
        await Subject.HandleAsync(command, CancellationToken.None);

        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
}