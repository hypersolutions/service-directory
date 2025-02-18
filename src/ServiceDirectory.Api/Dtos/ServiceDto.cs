using ServiceDirectory.Domain;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ServiceDirectory.Api.Dtos;

public sealed class ServiceDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required decimal Cost { get; init; }
    public IEnumerable<LocationDto> Locations { get; init; } = [];
    
    public static ServiceDto FromDomainModel(Service service)
    {
        return new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Cost = service.Cost,
            Locations = service.Locations.Select(LocationDto.FromDomainModel)
        };
    }
}