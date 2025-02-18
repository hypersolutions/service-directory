using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Queries.GetLocation;

public readonly struct GetLocationQuery(LocationId id)
{
    public LocationId Id { get; } = id;
}