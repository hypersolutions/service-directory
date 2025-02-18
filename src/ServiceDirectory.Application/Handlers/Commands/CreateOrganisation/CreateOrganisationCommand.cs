namespace ServiceDirectory.Application.Handlers.Commands.CreateOrganisation;

public readonly struct CreateOrganisationCommand(
    string name, 
    string description,
    string addressLine1,
    string? addressLine2,
    string townOrCity,
    string county,
    string postcode)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string AddressLine1 { get; } = addressLine1;
    public string? AddressLine2 { get; } = addressLine2;
    public string TownOrCity { get; } = townOrCity;
    public string County { get; } = county;
    public string Postcode { get; } = postcode;
}