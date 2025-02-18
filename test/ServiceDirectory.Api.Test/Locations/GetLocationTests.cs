using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Locations;

[Collection("Database")]
public class GetLocationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetLocationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownLocation_GetAsync_ReturnsNotFound()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/location/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownLocation_GetAsync_ReturnsServiceDetails()
    {
        var client = _factory.CreateClient();
        var serviceId = _factory.GetLocationId("City Hall");
        
        var response = await client.GetAsync($"/api/location/{serviceId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<LocationDto>();
        dto.ShouldBeSameAs(TestLocations.BristolCountyCouncil());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}