using ServiceDirectory.Domain.Primitives;
#pragma warning disable CS8618 // Affects EFCore

namespace ServiceDirectory.Domain;

public sealed class Service
{
    private readonly List<Location> _locations = [];

    public ServiceId Id { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Cost Cost { get; set; }
    public IEnumerable<Location> Locations => _locations;
    public StatusType Status { get; set; } = StatusType.Active;
    
    public void AddLocation(Location location) => _locations.Add(location);
}