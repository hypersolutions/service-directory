using System.Net;
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