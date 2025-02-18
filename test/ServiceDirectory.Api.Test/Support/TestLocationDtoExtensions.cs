using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Domain;
using Shouldly;

namespace ServiceDirectory.Api.Test.Support;

public static class TestLocationDtoExtensions
{
    public static void ShouldBeSameAs(this LocationDto dto, Location model)
    {
        dto.Id.ShouldBeGreaterThan(0);
        dto.AddressLine1.ShouldBe(model.AddressLine1);
        dto.AddressLine2.ShouldBe(model.AddressLine2);
        dto.TownOrCity.ShouldBe(model.TownOrCity);
        dto.County.ShouldBe(model.County);
        dto.Postcode.ShouldBe(model.Postcode);
        dto.Latitude.ShouldBe((double)model.Latitude);
        dto.Longitude.ShouldBe((double)model.Longitude);
    }
}