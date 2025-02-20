using System.Net;
using FastEndpoints;
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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidLocationId_PutAsync_ReturnsBadRequest(int locationId)
    {
        var location = TestLocations.BristolCountyCouncil();
        var request = new UpdateLocationRequest
        {
            Id = locationId,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location Id is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationAddressLine1_PutAsync_ReturnsBadRequest(string? addressLine1)
    {
        var location = TestLocations.AAMeeting();
        var locationId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = locationId,
            AddressLine1 = addressLine1!,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location address line 1 is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationTownOrCity_PutAsync_ReturnsBadRequest(string? townOrCity)
    {
        var location = TestLocations.AAMeeting();
        var locationId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = locationId,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = townOrCity!,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location town or city is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationCounty_PutAsync_ReturnsBadRequest(string? county)
    {
        var location = TestLocations.AAMeeting();
        var locationId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = locationId,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = county!,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location county is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationPostcode_PutAsync_ReturnsBadRequest(string? postcode)
    {
        var location = TestLocations.AAMeeting();
        var locationId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = locationId,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = postcode!
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location postcode is required.");
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
        var locationId = _factory.GetLocationId(location.AddressLine1);
        var request = new UpdateLocationRequest
        {
            Id = locationId,
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