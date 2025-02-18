using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.CreateService;

public readonly struct CreateServiceCommand(
    OrganisationId orgId, 
    string name, 
    string description, 
    decimal cost,
    string addressLine1,
    string? addressLine2,
    string townOrCity,
    string county,
    string postcode)
{
    public int OrganisationId { get; } = orgId;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public decimal Cost { get; } = cost;
    public string AddressLine1 { get; } = addressLine1;
    public string? AddressLine2 { get; } = addressLine2;
    public string TownOrCity { get; } = townOrCity;
    public string County { get; } = county;
    public string Postcode { get; } = postcode;
}