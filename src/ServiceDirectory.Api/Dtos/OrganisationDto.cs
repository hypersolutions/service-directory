using ServiceDirectory.Domain;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ServiceDirectory.Api.Dtos;

public sealed class OrganisationDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public IEnumerable<ServiceDto> Services { get; init; } = [];
    public IEnumerable<LocationDto> Locations { get; init; } = [];

    public static OrganisationDto FromDomainModel(Organisation organisation)
    {
        return new OrganisationDto
        {
            Id = organisation.Id,
            Name = organisation.Name,
            Description = organisation.Description,
            Services = organisation.Services.Select(ServiceDto.FromDomainModel),
            Locations = organisation.Locations.Select(LocationDto.FromDomainModel)
        };
    }
}