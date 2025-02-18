using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class GetOrganisationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetOrganisationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownOrganisation_GetAsync_ReturnsNotFound()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/organisation/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownOrganisation_GetAsync_ReturnsOrganisationDetails()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/organisation/1");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<OrganisationDto>();
        dto.ShouldBeSameAs(TestOrganisations.BristolCountyCouncil());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}