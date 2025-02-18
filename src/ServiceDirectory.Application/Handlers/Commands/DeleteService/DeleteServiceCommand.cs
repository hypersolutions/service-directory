using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteService;

public readonly struct DeleteServiceCommand(ServiceId id)
{
    public ServiceId Id { get; } = id;
}