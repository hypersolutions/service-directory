using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateLocation;

public readonly struct UpdateLocationCommand(
    LocationId? locationId,
    string addressLine1,
    string? addressLine2,
    string townOrCity,
    string county,
    string postcode)
{
    public LocationId? Id { get; } = locationId;
    public string AddressLine1 { get; } = addressLine1;
    public string? AddressLine2 { get; } = addressLine2;
    public string TownOrCity { get; } = townOrCity;
    public string County { get; } = county;
    public string Postcode { get; } = postcode;
}