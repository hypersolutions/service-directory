using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteLocation;

public readonly struct DeleteLocationCommand(LocationId id)
{
    public LocationId Id { get; } = id;
}