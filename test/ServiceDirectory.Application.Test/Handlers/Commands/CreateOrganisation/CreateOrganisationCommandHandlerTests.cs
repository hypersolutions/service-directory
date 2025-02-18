using NSubstitute;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Commands.CreateOrganisation;

public class CreateOrganisationCommandHandlerTests : TestBase<CreateOrganisationCommandHandler>
{
    private readonly Location _discoveryHomeLocation = TestLocations.DiscoveryHome();
    private readonly IApplicationRepository _repository;
    private readonly IPostcodeClient _postcodeClient;

    public CreateOrganisationCommandHandlerTests()
    {
        _repository = MockFor<IApplicationRepository>();
        _postcodeClient = MockFor<IPostcodeClient>();
    }

    [Fact]
    public async Task NoPostcodeFound_HandleAsync_ReturnsError()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Error("Postcode not found.", ErrorType.NotFound));
        
        var result = await Subject.HandleAsync(command, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(e => e is { Description: "Postcode not found.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_SavesRepositoryChanges()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        
        await Subject.HandleAsync(command, CancellationToken.None);

        await _repository.Received().AddAsync(Arg.Is<Organisation>(
            o => o.Name == command.Name && o.Description == command.Description));
        await _repository.Received().SaveChangesAsync(CancellationToken.None);
    }
    
    [Fact]
    public async Task WithOrganisationDetails_HandleAsync_ReturnsOrganisation()
    {
        var command = GetCreateOrganisationCommand();
        _postcodeClient.ResolvePostcodeLocationAsync(command.Postcode, CancellationToken.None).Returns(new Coordinate(_discoveryHomeLocation.Latitude, _discoveryHomeLocation.Longitude));
        
        var organisation = await Subject.HandleAsync(command, CancellationToken.None);

        await organisation.ShouldBeSuccessMatchAsync(
            o => o.Name == command.Name && o.Description == command.Description);
    }
    
    private CreateOrganisationCommand GetCreateOrganisationCommand() => new(
        "Org1", "Org1 description", _discoveryHomeLocation.AddressLine1, _discoveryHomeLocation.AddressLine2, 
        _discoveryHomeLocation.TownOrCity, _discoveryHomeLocation.County, _discoveryHomeLocation.Postcode);
}