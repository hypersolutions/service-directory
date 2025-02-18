using Scalar.AspNetCore;
// ReSharper disable UnusedMethodReturnValue.Global

namespace ServiceDirectory.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplication MapOpenApiUi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Service Directory API")
                .WithTheme(ScalarTheme.DeepSpace);
        });

        return app;
    }
}