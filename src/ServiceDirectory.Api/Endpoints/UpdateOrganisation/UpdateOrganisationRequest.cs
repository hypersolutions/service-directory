// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ServiceDirectory.Api.Endpoints.UpdateOrganisation;

public sealed class UpdateOrganisationRequest
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}