using ServiceDirectory.Domain.Primitives;
#pragma warning disable CS8618 // Affects EFCore

namespace ServiceDirectory.Domain;

public sealed class Location
{
    public LocationId Id { get; init; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required string TownOrCity { get; set; }
    public required string County { get; set; }
    public required string Postcode { get; set; }
    public required Latitude Latitude { get; set; }
    public required Longitude Longitude { get; set; }
    public StatusType Status { get; set; } = StatusType.Active;
}