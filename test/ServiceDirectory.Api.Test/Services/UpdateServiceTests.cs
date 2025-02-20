using System.Net;
using FastEndpoints;
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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidServiceId_PutAsync_ReturnsBadRequest(int serviceId)
    {
        var service = TestServices.AAMeeting();
        var request = new UpdateServiceRequest
        {
            Id = serviceId,
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service Id is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidServiceName_PutAsync_ReturnsBadRequest(string? serviceName)
    {
        var service = TestServices.AAMeeting();
        var request = new UpdateServiceRequest
        {
            Id = _factory.GetServiceId(service.Name),
            Name = serviceName!,
            Description = service.Description,
            Cost = service.Cost
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
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
    public async Task InvalidServiceDescription_PutAsync_ReturnsBadRequest(string? serviceDescription)
    {
        var service = TestServices.AAMeeting();
        var request = new UpdateServiceRequest
        {
            Id = _factory.GetServiceId(service.Name),
            Name = service.Name,
            Description = serviceDescription!,
            Cost = service.Cost
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service description is required.");
    }
    
    [Theory]
    [InlineData(-0.01)]
    [InlineData(2000.01)]
    public async Task InvalidServiceCost_PutAsync_ReturnsBadRequest(decimal serviceCost)
    {
        var service = TestServices.AAMeeting();
        var request = new UpdateServiceRequest
        {
            Id = _factory.GetServiceId(service.Name),
            Name = service.Name,
            Description = service.Description,
            Cost = serviceCost
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/service");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service cost must be between 0 (free) and £2,000.");
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