using System.Net;
using FastEndpoints;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Locations;

[Collection("Database")]
public class DeleteLocationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public DeleteLocationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidLocationId_PostAsync_ReturnsBadRequest(int locationId)
    {
        var client = _factory.CreateClient();
        
        var response = await client.DeleteAsync($"/api/location/{locationId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The location Id is invalid.");
    }
    
    [Fact]
    public async Task UnknownLocation_DeleteAsync_ReturnsNoContent()
    {
        var client = _factory.CreateClient();
        
        var response = await client.DeleteAsync("/api/location/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownLocation_DeleteAsync_DoesNotReturnLocationDetails()
    {
        var client = _factory.CreateClient();
        var locationId = _factory.GetLocationId("City Hall");
        
        var response = await client.DeleteAsync($"/api/location/{locationId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response = await client.GetAsync($"/api/location/{locationId}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}