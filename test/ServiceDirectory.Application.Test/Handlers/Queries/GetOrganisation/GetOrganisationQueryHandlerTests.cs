using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.GetOrganisation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetOrganisation;

public class GetOrganisationQueryHandlerTests : TestBase<GetOrganisationQueryHandler>
{
    private readonly Organisation _towerHamlets;
    
    public GetOrganisationQueryHandlerTests()
    {
        _towerHamlets = TestOrganisations.TowerHamlets();
        var repository = MockFor<IApplicationRepository>();
        repository.Organisations.Returns(new List<Organisation>([_towerHamlets]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownOrganisation_HandleAsync_ReturnsError()
    {
        var query = new GetOrganisationQuery(100);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the organisation for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownOrganisation_HandleAsync_ReturnsOrganisation()
    {
        var query = new GetOrganisationQuery(_towerHamlets.Id);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(o => o.Id == _towerHamlets.Id);
    }
}