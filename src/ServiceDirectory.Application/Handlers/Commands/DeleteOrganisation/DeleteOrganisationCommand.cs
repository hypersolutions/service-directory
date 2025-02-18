using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteOrganisation;

public readonly struct DeleteOrganisationCommand(OrganisationId id)
{
    public OrganisationId Id { get; } = id;
}