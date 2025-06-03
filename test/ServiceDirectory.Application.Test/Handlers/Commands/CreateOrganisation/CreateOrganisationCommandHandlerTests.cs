using RockHopper;
using RockHopper.Mocking;
using RockHopper.Mocking.Parameters;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.CreateOrganisation;

public class CreateOrganisationCommandHandlerTests
{
    private readonly CreateOrganisationCommandHandler _handler;
    private readonly Location _discoveryHomeLocation = TestLocations.DiscoveryHome();
    private readonly Mock<IApplicationRepository> _repository;
    private readonly Mock<IPostcodeClient> _postcodeClient;

    public CreateOrganisationCommandHandlerTests()
    {
        _handler = TestSubject.Create<CreateOrganisationCommandHandler>();
        _repository = _handler.GetMock<IApplicationRepository>();
        _postcodeClient = _handler.GetMock<IPostcodeClient>();
    }

    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.Function(r => r.AddAsync(Param.IsAny<Organisation>(), CancellationToken.None)).Returns(Task.CompletedTask);
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        
        await _handler.HandleAsync(command, CancellationToken.None);

        _handler.VerifyAll();
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_ReturnsOrganisation()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient
            .Function(c => c.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None))
            .Returns((Result<Coordinate>)new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        _repository.Function(r => r.AddAsync(Param.IsAny<Organisation>(), CancellationToken.None)).Returns(Task.CompletedTask);
        _repository.Function(r => r.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        
        var organisation = await _handler.HandleAsync(command, CancellationToken.None);

        await organisation.ShouldBeSuccessMatchAsync(
            o => o.Name == command.Name && o.Description == command.Description);
    }
    
    private CreateOrganisationCommand GetCreateOrganisationCommand() => new(
        "Org1", "Org1 description", _discoveryHomeLocation.AddressLine1, _discoveryHomeLocation.AddressLine2, 
        _discoveryHomeLocation.TownOrCity, _discoveryHomeLocation.County, _discoveryHomeLocation.Postcode);
}