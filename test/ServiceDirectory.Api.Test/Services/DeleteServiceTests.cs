﻿using System.Net;
using FastEndpoints;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;
using Shouldly;
using Xunit;

namespace ServiceDirectory.Api.Test.Services;

[Collection("Database")]
public class DeleteServiceTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public DeleteServiceTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.SetSeedDataAction(SeedData);
        _factory.SetRole(Roles.Admin);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidServiceId_PostAsync_ReturnsBadRequest(int serviceId)
    {
        var client = _factory.CreateClient();
        
        var response = await client.DeleteAsync($"/api/service/{serviceId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var error = await response.HttpResponseMessageAsync<ErrorResponse>();
        error.ShouldContainError("The service Id is invalid.");
    }
    
    [Fact]
    public async Task UnknownService_DeleteAsync_ReturnsNoContent()
    {
        var client = _factory.CreateClient();
        
        var response = await client.DeleteAsync("/api/service/100");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task KnownService_DeleteAsync_DoesNotReturnServiceDetails()
    {
        var client = _factory.CreateClient();
        var serviceId = _factory.GetServiceId("AA Meeting - Discussion Group");
        
        var response = await client.DeleteAsync($"/api/service/{serviceId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response = await client.GetAsync($"/api/service/{serviceId}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        context.Organisations.Add(TestOrganisations.BristolCountyCouncil());
    }
}