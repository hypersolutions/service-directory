using System.Net;
using FastEndpoints;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.CreateOrganisation;
using ServiceDirectory.Api.Test.Support;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class CreateOrganisationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public CreateOrganisationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationName_PostAsync_ReturnsBadRequest(string? organisationName)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisationName!,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation name is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationDescription_PostAsync_ReturnsBadRequest(string? organisationDescription)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisationDescription!,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation description is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationAddressLine1_PostAsync_ReturnsBadRequest(string? addressLine1)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = addressLine1!,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation address line 1 is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationTownOrCity_PostAsync_ReturnsBadRequest(string? townOrCity)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = townOrCity!,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation town or city is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationCounty_PostAsync_ReturnsBadRequest(string? county)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = county!,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation county is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationPostcode_PostAsync_ReturnsBadRequest(string? postcode)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = postcode!
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation postcode is required.");
    }
    
    [Fact]
    public async Task ForNewOrganisation_PostAsync_ReturnsNewOrganisationDetails()
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.HttpResponseMessageAsync<OrganisationDto>();
        dto.ShouldBeSameAs(organisation, checkServices: false);
    }
}