using System.Net;
using FastEndpoints;
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
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidOrganisationId_PutAsync_ReturnsBadRequest(int organisationId)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var request = new UpdateOrganisationRequest
        {
            Id = organisationId,
            Name = organisation.Name,
            Description = organisation.Description
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation Id is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationName_PutAsync_ReturnsBadRequest(string? organisationName)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var organisationId = _factory.GetOrganisationId(organisation.Name);
        var request = new UpdateOrganisationRequest
        {
            Id = organisationId,
            Name = organisationName!,
            Description = organisation.Description
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation name is required.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidOrganisationDescription_PutAsync_ReturnsBadRequest(string? organisationDescription)
    {
        var organisation = TestOrganisations.BristolCountyCouncil();
        var organisationId = _factory.GetOrganisationId(organisation.Name);
        var request = new UpdateOrganisationRequest
        {
            Id = organisationId,
            Name = organisation.Name,
            Description = organisationDescription!
        };
        var client = _factory.CreateClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/organisation");
        requestMessage.AddContent(request);
        
        var response = await client.SendAsync(requestMessage);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The organisation description is required.");
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