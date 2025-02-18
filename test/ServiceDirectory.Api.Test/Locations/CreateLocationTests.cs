using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.CreateLocation;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Locations;

[Collection("Database")]
public class CreateLocationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public CreateLocationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownOrganisation_PostAsync_ReturnsNoContent()
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 100,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task ForNewLocation_PostAsync_ReturnsNewLocationDetails()
    {
        var location = TestLocations.EastSussexCountyCouncil();
        var request = new CreateLocationRequest
        {
            OrganisationId = _factory.GetOrganisationId("East Sussex County Council"),
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.HttpResponseMessageAsync<LocationDto>();
        dto.ShouldBeSameAs(location);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.EastSussexCountyCouncil());
    }
}