using System.Net;
using FastEndpoints;
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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidOrganisationId_PostAsync_ReturnsBadRequest(int organisationId)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = organisationId,
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
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("Either service Id or organisation Id is required.");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidServiceId_PostAsync_ReturnsBadRequest(int serviceId)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            ServiceId = serviceId,
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
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("Either service Id or organisation Id is required.");
    }
    
    [Fact]
    public async Task BothOrganisationAndServiceIdProvided_PostAsync_ReturnsBadRequest()
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 10,
            ServiceId = 10,
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
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("Either service Id or organisation Id is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationAddressLine1_PostAsync_ReturnsBadRequest(string? addressLine1)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 100,
            AddressLine1 = addressLine1!,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The address line 1 is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationTownOrCity_PostAsync_ReturnsBadRequest(string? townOrCity)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 100,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = townOrCity!,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The town or city is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationCounty_PostAsync_ReturnsBadRequest(string? county)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 100,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = county!,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The county is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLocationPostcode_PostAsync_ReturnsBadRequest(string? postcode)
    {
        var location = TestLocations.AAMeeting();
        var request = new CreateLocationRequest
        {
            OrganisationId = 100,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = postcode!
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/location");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The postcode is required.");
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