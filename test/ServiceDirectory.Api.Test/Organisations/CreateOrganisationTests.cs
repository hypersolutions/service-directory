using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.CreateOrganisation;
using ServiceDirectory.Api.Test.Support;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class CreateOrganisationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public CreateOrganisationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task ForNewOrganisation_PostAsync_ReturnsNewOrganisationDetails()
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var location = organisation.Locations.First();
        var request = new CreateOrganisationRequest
        {
            Name = organisation.Name,
            Description = organisation.Description,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var dto = await response.HttpResponseMessageAsync<OrganisationDto>();
        dto.ShouldBeSameAs(organisation, checkServices: false);
    }
}