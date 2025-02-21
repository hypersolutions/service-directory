using FastEndpoints;
using Scalar.AspNetCore;
// ReSharper disable UnusedMethodReturnValue.Global

namespace ServiceDirectory.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapOpenApiUi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return app;
        
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.DeepSpace);
            options.Authentication = new ScalarAuthenticationOptions { PreferredSecurityScheme = "Bearer" };
        });

        return app;
    }

    public static WebApplication UseApiFastEndpoints(this WebApplication app)
    {
        app.UseAuthentication().UseAuthorization().UseFastEndpoints();
        return app;
    }
}