using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Api.Test.Support;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Domain;
using ServiceDirectory.Infrastructure.Clients;
using ServiceDirectory.Infrastructure.Database;

// ReSharper disable ClassNeverInstantiated.Global

namespace ServiceDirectory.Api.Test;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly List<TestPostcodeLookup> _testPostcodeLookups = [];
    private Action<ApplicationDbContext> _seedAction = _ => { };
    private string? _role;
    private const string BearerSigningKey = "8ca984ec355e470cad63b468bbca73c5";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(SetConfiguration);
        builder.ConfigureServices(OverrideServices);
    }
    
    protected override void ConfigureClient(HttpClient client)
    {
        if (_role is not null)
        {
            var token = BearerTokenGenerator.CreateTestToken(BearerSigningKey, _role);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

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
    
    public void SetPostcodeLookup(HttpStatusCode statusCode, Location location)
    {
        var lookup = new TestPostcodeLookup(location.Postcode, statusCode,
            new TestPostcodeResult(location.Latitude, location.Longitude));
        _testPostcodeLookups.Add(lookup);
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
    
    private void OverrideServices(IServiceCollection services)
    {
        InitialisePostcodeClient(services);
        InitialiseDatabase(services);
    }
    
    private void InitialisePostcodeClient(IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IPostcodeClient));

        if (serviceDescriptor == null) return;
        
        services.Remove(serviceDescriptor);
        services.AddTransient(_ => new TestPostcodeInterceptingDelegatingHandler(_testPostcodeLookups));
            
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var postcodeUrl = configuration["PostcodeUrl"]!;
        services
            .AddHttpClient<IPostcodeClient, PostcodeClient>(c => c.BaseAddress = new Uri(postcodeUrl))
            .AddHttpMessageHandler<TestPostcodeInterceptingDelegatingHandler>();
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
            { "BearerSigningKey", BearerSigningKey },
            { "PostcodeUrl", "https://localhost:1234/postcode" }
        };
        builder.AddInMemoryCollection(settings);
    }
}