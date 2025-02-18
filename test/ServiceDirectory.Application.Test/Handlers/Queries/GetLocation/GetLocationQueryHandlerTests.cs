using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.GetLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Xunit;
// ReSharper disable MethodHasAsyncOverload

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetLocation;

public class GetLocationQueryHandlerTests : TestBase<GetLocationQueryHandler>
{
    private readonly Location _discoveryHome;
    
    public GetLocationQueryHandlerTests()
    {
        _discoveryHome = TestLocations.DiscoveryHome();
        var repository = MockFor<IApplicationRepository>();
        repository.Locations.Returns(new List<Location>([_discoveryHome]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ReturnsError()
    {
        var query = new GetLocationQuery(100);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        result.ShouldBeErrorMatch(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_ReturnsLocation()
    {
        var query = new GetLocationQuery(_discoveryHome.Id);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(o => o.Id == _discoveryHome.Id);
    }
}