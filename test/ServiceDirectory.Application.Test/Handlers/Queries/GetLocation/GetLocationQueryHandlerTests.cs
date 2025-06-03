using RockHopper;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.GetLocation;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Xunit;
// ReSharper disable MethodHasAsyncOverload

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetLocation;

public class GetLocationQueryHandlerTests
{
    private readonly GetLocationQueryHandler _handler;
    private readonly Location _discoveryHome;
    
    public GetLocationQueryHandlerTests()
    {
        _handler = TestSubject.Create<GetLocationQueryHandler>();
        _discoveryHome = TestLocations.DiscoveryHome();
        var repository = _handler.GetMock<IApplicationRepository>();
        repository.GetProperty(r => r.Locations).Returns(new List<Location>([_discoveryHome]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownLocation_HandleAsync_ReturnsError()
    {
        var query = new GetLocationQuery(100);
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        result.ShouldBeErrorMatch(
            e => e is { Description: "Unable to find the location for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownLocation_HandleAsync_ReturnsLocation()
    {
        var query = new GetLocationQuery(_discoveryHome.Id);
        
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(o => o.Id == _discoveryHome.Id);
    }
}