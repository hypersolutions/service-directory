using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class GetOrganisationsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetOrganisationsTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task HasOrganisations_GetAsync_ReturnsOrganisationDetailList()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/organisation");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<List<OrganisationDto>>();
        dto.ShouldBeSameAs(TestOrganisations.BristolCountyCouncil(), TestOrganisations.SouthamptonCountyCouncil());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
        context.Organisations.Add(TestOrganisations.SouthamptonCountyCouncil());
    }
}