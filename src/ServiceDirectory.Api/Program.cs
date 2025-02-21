using ServiceDirectory.Api.Extensions;
using ServiceDirectory.Application.Extensions;
using ServiceDirectory.Infrastructure.Extensions;
// ReSharper disable ClassNeverInstantiated.Global

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApiFastEndpoints(builder.Configuration);
builder.Services.AddSecureOpenApi();

var app = builder.Build();
app.MapOpenApiUi();
app.UseHttpsRedirection();
app.UseApiFastEndpoints();
app.MapGet("/", () => Results.Redirect("/scalar"));
app.Run();

public partial class Program;