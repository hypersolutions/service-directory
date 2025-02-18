using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Application.Handlers;
using ServiceDirectory.Application.Handlers.Commands.CreateLocation;
using ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;
using ServiceDirectory.Application.Handlers.Commands.CreateService;
using ServiceDirectory.Application.Handlers.Commands.DeleteLocation;
using ServiceDirectory.Application.Handlers.Commands.DeleteOrganisation;
using ServiceDirectory.Application.Handlers.Commands.DeleteService;
using ServiceDirectory.Application.Handlers.Commands.UpdateLocation;
using ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;
using ServiceDirectory.Application.Handlers.Commands.UpdateService;
using ServiceDirectory.Application.Handlers.Queries;
using ServiceDirectory.Application.Handlers.Queries.GetLocation;
using ServiceDirectory.Application.Handlers.Queries.GetOrganisation;
using ServiceDirectory.Application.Handlers.Queries.GetOrganisations;
using ServiceDirectory.Application.Handlers.Queries.GetService;
using ServiceDirectory.Application.Handlers.Queries.GetServices;
using ServiceDirectory.Application.Handlers.Queries.ServiceSearch;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable UnusedMethodReturnValue.Global

namespace ServiceDirectory.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers();
        return services;
    }
    
    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddTransient<IHandler<NoopQuery, Result<IEnumerable<Service>>>, GetServicesQueryHandler>();
        services.AddTransient<IHandler<GetOrganisationQuery, Result<Organisation>>, GetOrganisationQueryHandler>();
        services.AddTransient<IHandler<NoopQuery, Result<IEnumerable<Organisation>>>, GetOrganisationsQueryHandler>();
        services.AddTransient<IHandler<CreateOrganisationCommand, Result<Organisation>>, CreateOrganisationCommandHandler>();
        services.AddTransient<IHandler<UpdateOrganisationCommand, Result<Organisation>>, UpdateOrganisationCommandHandler>();
        services.AddTransient<IHandler<DeleteOrganisationCommand, Result<Organisation>>, DeleteOrganisationCommandHandler>();
        services.AddTransient<IHandler<GetServiceQuery, Result<Service>>, GetServiceQueryHandler>();
        services.AddTransient<IHandler<ServiceSearchQuery, Result<IEnumerable<ServiceDetail>>>, ServiceSearchQueryHandler>();
        services.AddTransient<IHandler<CreateServiceCommand, Result<Service>>, CreateServiceCommandHandler>();
        services.AddTransient<IHandler<UpdateServiceCommand, Result<Service>>, UpdateServiceCommandHandler>();
        services.AddTransient<IHandler<DeleteServiceCommand, Result<Service>>, DeleteServiceCommandHandler>();
        services.AddTransient<IHandler<GetLocationQuery, Result<Location>>, GetLocationQueryHandler>();
        services.AddTransient<IHandler<CreateLocationCommand, Result<Location>>, CreateLocationCommandHandler>();
        services.AddTransient<IHandler<UpdateLocationCommand, Result<Location>>, UpdateLocationCommandHandler>();
        services.AddTransient<IHandler<DeleteLocationCommand, Result<Location>>, DeleteLocationCommandHandler>();
        return services;
    }
}