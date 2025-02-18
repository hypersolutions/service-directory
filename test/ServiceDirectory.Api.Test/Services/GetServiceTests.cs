using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class GetServiceTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetServiceTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownService_GetAsync_ReturnsNotFound()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/service/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownService_GetAsync_ReturnsServiceDetails()
    {
        var client = _factory.CreateClient();
        var serviceId = _factory.GetServiceId("AA Meeting - Discussion Group");
        
        var response = await client.GetAsync($"/api/service/{serviceId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<ServiceDto>();
        dto.ShouldBeSameAs(TestServices.AAMeeting());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}