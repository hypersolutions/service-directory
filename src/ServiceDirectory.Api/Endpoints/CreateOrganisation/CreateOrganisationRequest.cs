// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ServiceDirectory.Api.Endpoints.CreateOrganisation;

public sealed class CreateOrganisationRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public required string TownOrCity { get; init; }
    public required string County { get; init; }
    public required string Postcode { get; init; }
}