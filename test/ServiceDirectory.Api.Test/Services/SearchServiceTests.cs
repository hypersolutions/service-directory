using System.Net;
using System.Web;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class SearchServiceTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public SearchServiceTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
        _factory.SetPostcodeLookup(HttpStatusCode.OK, TestLocations.AAMeeting());
        _factory.SetPostcodeLookup(HttpStatusCode.OK, TestLocations.TowerHamlets());
    }
    
    [Fact]
    public async Task ServiceOutsidePostcodeRange_GetAsync_ReturnsEmptyServiceList()
    {
        var client = _factory.CreateClient();
        var location = TestLocations.TowerHamlets();
        
        var response = await client.GetAsync(
            $"/api/service/search?postcode={HttpUtility.UrlPathEncode(location.Postcode)}&distance=5&maxResults=5");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<List<ServiceDetailDto>>();
        dto.Count.ShouldBe(0);
    }
    
    [Fact]
    public async Task ServiceWithinPostcodeRange_GetAsync_ReturnsServiceList()
    {
        var client = _factory.CreateClient();
        var service = TestServices.AAMeeting();
        var location = service.Locations.First();
        
        var response = await client.GetAsync(
            $"/api/service/search?postcode={HttpUtility.UrlPathEncode(location.Postcode)}&distance=5&maxResults=5");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<List<ServiceDetailDto>>();
        dto.Count.ShouldBe(1);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
        context.Organisations.Add(TestOrganisations.SouthamptonCountyCouncil());
    }
}