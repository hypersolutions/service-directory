using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.UpdateLocation;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Locations;

[Collection("Database")]
public class UpdateLocationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public UpdateLocationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownLocation_PutAsync_ReturnsNoContent()
    {
        var location = TestLocations.BristolCountyCouncil();
        var request = new UpdateLocationRequest
        {
            Id = 100,
            AddressLine1 = location.AddressLine1 + " Updated",
            AddressLine2 = location.AddressLine2 + " Updated",
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownService_PutAsync_ReturnsUpdatedServiceDetails()
    {
        var location = TestLocations.BristolCountyCouncil();
        var serviceId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = serviceId,
            AddressLine1 = location.AddressLine1 + " Updated",
            AddressLine2 = location.AddressLine2 + " Updated",
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<LocationDto>();
        dto.AddressLine1.ShouldBe(request.AddressLine1);
        dto.AddressLine2.ShouldBe(request.AddressLine2);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}