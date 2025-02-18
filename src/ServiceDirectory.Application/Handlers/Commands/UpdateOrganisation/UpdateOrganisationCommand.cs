using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;

public readonly struct UpdateOrganisationCommand(OrganisationId id, string name, string description)
{
    public OrganisationId Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
}