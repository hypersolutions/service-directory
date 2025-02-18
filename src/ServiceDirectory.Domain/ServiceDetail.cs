using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Domain;

public sealed class ServiceDetail
{
    public ServiceDetail(Organisation organisation, Service service, Location location, Distance distance)
    {
        OrganisationName = organisation.Name;
        Id = service.Id;
        Name = service.Name;
        Description = service.Description;
        Cost = service.Cost;
        HowFarAway = distance;
        AddressLine1 = location.AddressLine1;
        AddressLine2 = location.AddressLine2;
        TownOrCity = location.TownOrCity;
        County = location.County;
        Postcode = location.Postcode;
    }
    
    public string OrganisationName { get; }
    public ServiceId Id { get; }
    public string Name { get; }
    public string Description { get; }
    public Cost Cost { get; } 
    public Distance HowFarAway { get; }
    public string AddressLine1 { get; }
    public string? AddressLine2 { get; }
    public string TownOrCity { get; }
    public string County { get; }
    public string Postcode { get; }
}