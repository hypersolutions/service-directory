// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ServiceDirectory.Api.Endpoints.CreateLocation;

public sealed class CreateLocationRequest
{
    public int? ServiceId { get; init; }
    public int? OrganisationId { get; init; }
    public required string AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public required string TownOrCity { get; init; }
    public required string County { get; init; }
    public required string Postcode { get; init; }
}