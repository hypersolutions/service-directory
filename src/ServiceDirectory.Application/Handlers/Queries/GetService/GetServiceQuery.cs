using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Queries.GetService;

public readonly struct GetServiceQuery(ServiceId id)
{
    public ServiceId Id { get; } = id;
}