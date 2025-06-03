using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Handlers.Queries.GetServices;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using Xunit;
// ReSharper disable MethodHasAsyncOverload

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetServices;

public class GetServicesQueryHandlerTests
{
    [Fact]
    public async Task OrganisationsWithServices_HandleAsync_ReturnsAggregatedServices()
    {
        var handler = TestSubject.Create<GetServicesQueryHandler>();
        var towerHamlets = TestOrganisations.TowerHamlets();
        var cityOfLondon = TestOrganisations.CityOfLondon();
        var repository = handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Organisations).Returns(new[] { towerHamlets, cityOfLondon }.AsTestQueryable());
        
        var result = await handler.HandleAsync(new NoopQuery(), CancellationToken.None);

        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == towerHamlets.Services.First().Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == cityOfLondon.Services.First().Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == cityOfLondon.Services.Last().Id));
    }
}