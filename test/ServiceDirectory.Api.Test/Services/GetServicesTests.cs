using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class GetServicesTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetServicesTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task HasServices_GetAsync_ReturnsServiceDetailList()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/api/service");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<List<ServiceDto>>();
        dto.ShouldBeSameAs(TestServices.AAMeeting(), TestServices.SpecialEducationalNeedsTeam());
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
        context.Organisations.Add(TestOrganisations.SouthamptonCountyCouncil());
    }
}