using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Infrastructure.Database;
// ReSharper disable ClassNeverInstantiated.Global

namespace ServiceDirectory.Api.Test;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private Action<ApplicationDbContext> _seedAction = _ => { };
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            SetConfiguration(services);
            InitialiseDatabase(services);
        });
    }
    
    public void SetSeedDataAction(Action<ApplicationDbContext> action)
    {
        _seedAction = action;
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
    
    private static void SetConfiguration(IServiceCollection services)
    {
        var settings = new Dictionary<string, string?> { {"DatabaseConnection", "Data Source=sd.test.db"} };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        services.AddSingleton<IConfiguration>(configuration);
    }
}