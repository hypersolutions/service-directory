using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

// ReSharper disable UnusedMethodReturnValue.Global

namespace ServiceDirectory.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiFastEndpoints(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthenticationJwtBearer(o =>
            {
                var bearerSigningKey = configuration["BearerSigningKey"] 
                                       ?? throw new ArgumentException("Unable to find the bearer signing key.");
                o.SigningKey = bearerSigningKey;
            })
            .AddAuthorization(o => o.AddPolicy("Admin", b => b.RequireClaim("role", "admin")))
            .AddFastEndpoints();
        return services;
    }

    public static IServiceCollection AddSecureOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            options.AddDocumentTransformer<OpenApiSecuritySchemeTransformer>();
        });

        return services;
    }
    
    private sealed class OpenApiSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(
            OpenApiDocument document, 
            OpenApiDocumentTransformerContext context, 
            CancellationToken cancellationToken)
        {
            document.Info.Title = "Service Directory API";
            document.Info.Description = "Simple demo service directory API.";
            var securitySchema = CreateOpenApiSecurityScheme();
            var securityRequirement = CreateOpenApiSecurityRequirement();
            document.SecurityRequirements.Add(securityRequirement);
            document.Components = CreateOpenApiComponents(securitySchema);
            return Task.CompletedTask;
        }
        
        private static OpenApiSecurityScheme CreateOpenApiSecurityScheme() => 
            new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            };

        private static OpenApiSecurityRequirement CreateOpenApiSecurityRequirement() =>
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
                    },
                    []
                }
            };
        
        private static OpenApiComponents CreateOpenApiComponents(OpenApiSecurityScheme securitySchema) => 
            new()
            {
                SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                {
                    { "Bearer", securitySchema }
                }
            };
    }
}