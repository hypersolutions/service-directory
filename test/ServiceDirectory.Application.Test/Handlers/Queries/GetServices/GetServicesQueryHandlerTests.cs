using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Handlers.Queries.GetServices;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using Xunit;
// ReSharper disable MethodHasAsyncOverload

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetServices;

public class GetServicesQueryHandlerTests : TestBase<GetServicesQueryHandler>
{
    [Fact]
    public async Task OrganisationsWithServices_HandleAsync_ReturnsAggregatedServices()
    {
        var towerHamlets = TestOrganisations.TowerHamlets();
        var cityOfLondon = TestOrganisations.CityOfLondon();
        var repository = MockFor<IApplicationRepository>();
        repository.Organisations.Returns(new[] { towerHamlets, cityOfLondon }.AsTestQueryable());
        
        var result = await Subject.HandleAsync(new NoopQuery(), CancellationToken.None);

        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == towerHamlets.Services.First().Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == cityOfLondon.Services.First().Id));
        result.ShouldBeSuccessMatch(r => r.Any(s => s.Id == cityOfLondon.Services.Last().Id));
    }
}