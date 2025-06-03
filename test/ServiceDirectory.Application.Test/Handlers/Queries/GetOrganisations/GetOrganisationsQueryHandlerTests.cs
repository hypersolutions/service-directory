using RockHopper;
using RockHopper.Mocking;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Handlers.Queries.GetOrganisations;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetOrganisations;

public class GetOrganisationsQueryHandlerTests
{
    private readonly GetOrganisationsQueryHandler _handler;
    private readonly Mock<IApplicationRepository> _repository;
    
    public GetOrganisationsQueryHandlerTests()
    {
        _handler = TestSubject.Create<GetOrganisationsQueryHandler>();
        _repository = _handler.GetMock<IApplicationRepository>();
    }
    
    [Fact]
    public async Task Services_HandleAsync_ReturnsOrganisationsAndServices()
    {
        var towerHamlets = TestOrganisations.TowerHamlets();
        var cityOfLondon = TestOrganisations.CityOfLondon();
        _repository.GetProperty(r => r.Organisations).Returns(new List<Organisation>([towerHamlets, cityOfLondon]).AsTestQueryable());
        
        var result = await _handler.HandleAsync(new NoopQuery(), CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(o => o.Any(x => x.Id == towerHamlets.Id && x.Services.Any()));
        await result.ShouldBeSuccessMatchAsync(o => o.Any(x => x.Id == cityOfLondon.Id && x.Services.Any()));
    }
}