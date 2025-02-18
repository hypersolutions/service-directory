// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ServiceDirectory.Api.Endpoints.UpdateLocation;

public sealed class UpdateLocationRequest
{
    public int? Id { get; init; }
    public required string AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public required string TownOrCity { get; init; }
    public required string County { get; init; }
    public required string Postcode { get; init; }
}