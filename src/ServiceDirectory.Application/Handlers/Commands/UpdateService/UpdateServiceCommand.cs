using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateService;

public readonly struct UpdateServiceCommand(ServiceId serviceId, string name, string description, Cost cost)
{
    public int Id { get; } = serviceId;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public Cost Cost { get; } = cost;
}