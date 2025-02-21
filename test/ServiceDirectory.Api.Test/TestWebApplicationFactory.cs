using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Infrastructure.Database;

// ReSharper disable ClassNeverInstantiated.Global

namespace ServiceDirectory.Api.Test;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private Action<ApplicationDbContext> _seedAction = _ => { };
    private string _role = "admin";
    private const string BearerSigningKey = "8ca984ec355e470cad63b468bbca73c5";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(SetConfiguration);
        builder.ConfigureServices(InitialiseDatabase);
    }
    
    protected override void ConfigureClient(HttpClient client)
    {
        var token = BearerTokenGenerator.CreateTestToken(BearerSigningKey, _role);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        base.ConfigureClient(client);
    }
    
    public void SetSeedDataAction(Action<ApplicationDbContext> action)
    {
        _seedAction = action;
    }

    public void SetRole(string role)
    {
        _role = role;
    }
    
    public int GetOrganisationId(string name)
    {
        using var scope = Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ApplicationDbContext>();
        return context.Organisations.FirstOrDefault(o => o.Name == name)?.Id ?? 0;
    }

    public int GetServiceId(string name)
    {
        using var scope = Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ApplicationDbContext>();
        return context.Services.FirstOrDefault(o => o.Name == name)?.Id ?? 0;
    }
    
    public int GetLocationId(string addressLine1)
    {
        using var scope = Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ApplicationDbContext>();
        return context.Locations.FirstOrDefault(o => o.AddressLine1 == addressLine1)?.Id ?? 0;
    }
    
    private void InitialiseDatabase(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
            
        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        _seedAction(context);
        context.SaveChanges();
    }
    
    private static void SetConfiguration(IConfigurationBuilder builder)
    {
        var settings = new Dictionary<string, string?>
        {
            { "DatabaseConnection", "Data Source=sd.test.db" },
            { "BearerSigningKey", BearerSigningKey }
        };
        builder.AddInMemoryCollection(settings);
    }
}