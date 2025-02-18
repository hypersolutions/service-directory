using FastEndpoints;
using ServiceDirectory.Api.Extensions;
using ServiceDirectory.Application.Extensions;
using ServiceDirectory.Infrastructure.Extensions;
// ReSharper disable ClassNeverInstantiated.Global

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddFastEndpoints();
builder.Services.AddOpenApi();

var app = builder.Build();
app.MapOpenApiUi();
app.UseHttpsRedirection();
app.UseFastEndpoints();
app.MapGet("/", () => Results.Redirect("/scalar"));
app.Run();

public partial class Program;