using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.UpdateService;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class UpdateServiceTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public UpdateServiceTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownService_PutAsync_ReturnsNoContent()
    {
        var service = TestServices.AAMeeting();
        var request = new UpdateServiceRequest
        {
            Id = 100,
            Name = service.Name + " Updated",
            Description = service.Description + " Updated"
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownService_PutAsync_ReturnsUpdatedServiceDetails()
    {
        var service = TestServices.AAMeeting();
        var serviceId = _factory.GetServiceId(service.Name);
        var request = new UpdateServiceRequest
        {
            Id = serviceId,
            Name = service.Name + " Updated",
            Description = service.Description + " Updated"
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<ServiceDto>();
        dto.Name.ShouldBe(request.Name);
        dto.Description.ShouldBe(request.Description);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}