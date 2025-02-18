using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.CreateLocation;

public readonly struct CreateLocationCommand(
    ServiceId? serviceId,
    OrganisationId? organisationId,
    string addressLine1,
    string? addressLine2,
    string townOrCity,
    string county,
    string postcode)
{
    public ServiceId? ServiceId { get; } = serviceId;
    public OrganisationId? OrganisationId { get; } = organisationId;
    public string AddressLine1 { get; } = addressLine1;
    public string? AddressLine2 { get; } = addressLine2;
    public string TownOrCity { get; } = townOrCity;
    public string County { get; } = county;
    public string Postcode { get; } = postcode;
}