using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Queries.GetOrganisation;

public readonly struct GetOrganisationQuery(OrganisationId id)
{
    public OrganisationId Id { get; } = id;
}