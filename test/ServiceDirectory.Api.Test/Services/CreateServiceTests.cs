using System.Net;
using FastEndpoints;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.CreateService;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class CreateServiceTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public CreateServiceTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidOrganisationId_PostAsync_ReturnsBadRequest(int organisationId)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = organisationId,
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation Id is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceName_PostAsync_ReturnsBadRequest(string? serviceName)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = serviceName!,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service name is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceDescription_PostAsync_ReturnsBadRequest(string? serviceDescription)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = serviceDescription!,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service description is required.");
    }
    
    [Theory]
    [InlineData(-0.01)]
    [InlineData(2000.01)]
    public async Task InvalidServiceCost_PostAsync_ReturnsBadRequest(decimal serviceCost)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = serviceCost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service cost must be between 0 (free) and £2,000.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceAddressLine1_PostAsync_ReturnsBadRequest(string? addressLine1)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = addressLine1!,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service address line 1 is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceTownOrCity_PostAsync_ReturnsBadRequest(string? townOrCity)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = townOrCity!,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service town or city is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceCounty_PostAsync_ReturnsBadRequest(string? county)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = county!,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service county is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServicePostcode_PostAsync_ReturnsBadRequest(string? postcode)
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = postcode!
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service postcode is required.");
    }
    
    [Fact]
    public async Task UnknownOrganisation_PostAsync_ReturnsNoContent()
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = 100,
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task ForNewService_PostAsync_ReturnsNewServiceDetails()
    {
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        var request = new CreateServiceRequest
        {
            OrganisationId = _factory.GetOrganisationId("Tower Hamlets"),
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.HttpResponseMessageAsync<ServiceDto>();
        dto.ShouldBeSameAs(service);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.TowerHamlets());
    }
}