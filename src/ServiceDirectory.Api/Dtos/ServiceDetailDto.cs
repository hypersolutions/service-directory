using ServiceDirectory.Domain;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ServiceDirectory.Api.Dtos;

public sealed class ServiceDetailDto
{
    public required string OrganisationName { get; init; }
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required decimal Cost { get; init; } 
    public required double HowFarAway { get; init; }
    public required string AddressLine1 { get; init; }
    public required string? AddressLine2 { get; init; }
    public required string TownOrCity { get; init; }
    public required string County { get; init; }
    public required string Postcode { get; init; }

    public static ServiceDetailDto FromDomainModel(ServiceDetail serviceDetail)
    {
        return new ServiceDetailDto
        {
            OrganisationName = serviceDetail.OrganisationName,
            Id = serviceDetail.Id,
            Name = serviceDetail.Name,
            Description = serviceDetail.Description,
            Cost = serviceDetail.Cost,
            HowFarAway = Math.Round(serviceDetail.HowFarAway, 1),
            AddressLine1 = serviceDetail.AddressLine1,
            AddressLine2 = serviceDetail.AddressLine2,
            TownOrCity = serviceDetail.TownOrCity,
            County = serviceDetail.County,
            Postcode = serviceDetail.Postcode
        };
    }
}