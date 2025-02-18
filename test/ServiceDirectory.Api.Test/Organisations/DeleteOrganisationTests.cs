using System.Net;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class DeleteOrganisationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public DeleteOrganisationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownOrganisation_DeleteAsync_ReturnsNoContent()
    {
        var client = _factory.CreateClient();
        
        var response = await client.DeleteAsync("/api/organisation/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownOrganisation_DeleteAsync_DoesNotReturnOrganisationDetails()
    {
        var client = _factory.CreateClient();
        var organisationId = _factory.GetOrganisationId("Bristol County Council");
        
        var response = await client.DeleteAsync($"/api/organisation/{organisationId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response = await client.GetAsync($"/api/organisation/{organisationId}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}