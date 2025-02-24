using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Infrastructure.Clients;
using ServiceDirectory.Infrastructure.Database;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMethodReturnValue.Local

namespace ServiceDirectory.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly Random JitterGenerator = new();
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddClients(configuration);
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            var connectionString = configuration["DatabaseConnection"] ?? throw new ArgumentException("Unable to find the database connection.");
            o.UseSqlite(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString()));
            o.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        var postcodeUrl = configuration["PostcodeUrl"] ?? throw new ArgumentException("Unable to find the postcode URL.");
        var policy = HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(6, SleepDurationProvider);
        services
            .AddHttpClient<IPostcodeClient, PostcodeClient>(c => c.BaseAddress = new Uri(postcodeUrl))
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(policy);
        return services;
    }
    
    private static TimeSpan SleepDurationProvider(int retryAttempt) => ExponentialBackOff(retryAttempt) + Jitter(JitterGenerator);
    
    private static TimeSpan ExponentialBackOff(int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

    private static TimeSpan Jitter(Random jitter) => TimeSpan.FromMilliseconds(jitter.Next(0, 1000));
}