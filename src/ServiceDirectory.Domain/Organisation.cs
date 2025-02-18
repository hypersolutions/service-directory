using ServiceDirectory.Domain.Primitives;
#pragma warning disable CS8618 // Affects EFCore

namespace ServiceDirectory.Domain;

public sealed class Organisation
{
    private readonly List<Service> _services = [];
    private readonly List<Location> _locations = [];

    public OrganisationId Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public StatusType Status { get; set; } = StatusType.Active;
    public IEnumerable<Service> Services => _services;
    public IEnumerable<Location> Locations => _locations;

    public void AddService(Service service) => _services.Add(service);
    
    public void AddLocation(Location location) => _locations.Add(location);
}