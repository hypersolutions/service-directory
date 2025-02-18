using System.Net;
using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Api.Endpoints.UpdateOrganisation;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Organisations;

[Collection("Database")]
public class UpdateOrganisationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public UpdateOrganisationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
    }
    
    [Fact]
    public async Task UnknownOrganisation_PutAsync_ReturnsNoContent()
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var request = new UpdateOrganisationRequest
        {
            Id = 100,
            Name = organisation.Name + " Updated",
            Description = organisation.Description + " Updated"
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownOrganisation_PutAsync_ReturnsUpdatedOrganisationDetails()
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var organisationId = _factory.GetOrganisationId(organisation.Name);
        var request = new UpdateOrganisationRequest
        {
            Id = organisationId,
            Name = organisation.Name + " Updated",
            Description = organisation.Description + " Updated"
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.HttpResponseMessageAsync<OrganisationDto>();
        dto.Name.ShouldBe(request.Name);
        dto.Description.ShouldBe(request.Description);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}