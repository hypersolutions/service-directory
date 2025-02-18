using NSubstitute;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Handlers.Queries.GetService;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Application.Test.Support;
using ServiceDirectory.Application.Test.Support.MockQueryable;
using ServiceDirectory.Domain;
using Xunit;

namespace ServiceDirectory.Application.Test.Handlers.Queries.GetService;

public class GetServiceQueryHandlerTests : TestBase<GetServiceQueryHandler>
{
    private readonly Service _discoveryHome;
    
    public GetServiceQueryHandlerTests()
    {
        _discoveryHome = TestServices.DiscoveryHome();
        var repository = MockFor<IApplicationRepository>();
        repository.Services.Returns(new List<Service>([_discoveryHome]).AsTestQueryable());
    }
    
    [Fact]
    public async Task UnknownService_HandleAsync_ReturnsError()
    {
        var query = new GetServiceQuery(100);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeErrorMatchAsync(
            e => e is { Description: "Unable to find the service for 100.", Type: ErrorType.NotFound });
    }
    
    [Fact]
    public async Task KnownService_HandleAsync_ReturnsService()
    {
        var query = new GetServiceQuery(_discoveryHome.Id);
        
        var result = await Subject.HandleAsync(query, CancellationToken.None);

        await result.ShouldBeSuccessMatchAsync(o => o.Id == _discoveryHome.Id);
    }
}