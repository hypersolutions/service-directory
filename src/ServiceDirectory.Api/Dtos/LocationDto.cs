using ServiceDirectory.Domain;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ServiceDirectory.Api.Dtos;

public sealed class LocationDto
{
    public int Id { get; init; }
    public required string AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public required string TownOrCity { get; init; }
    public required string County { get; init; }
    public required string Postcode { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public static LocationDto FromDomainModel(Location location)
    {
        return new LocationDto
        {
            Id = location.Id,
            AddressLine1 = location.AddressLine1,
            AddressLine2 = location.AddressLine2,
            TownOrCity = location.TownOrCity,
            County = location.County,
            Postcode = location.Postcode,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        };
    }
}