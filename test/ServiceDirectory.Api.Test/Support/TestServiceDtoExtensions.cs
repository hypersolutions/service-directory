using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Domain;
using Shouldly;

namespace ServiceDirectory.Api.Test.Support;

public static class TestServiceDtoExtensions
{
    public static void ShouldBeSameAs(this ServiceDto dto, Service model)
    {
        dto.Id.ShouldBeGreaterThan(0);
        dto.Name.ShouldBe(model.Name);
        dto.Description.ShouldBe(model.Description);
        dto.Cost.ShouldBe((decimal)model.Cost);
        dto.Locations.Count().ShouldBe(model.Locations.Count());
        
        for (var i = 0; i < dto.Locations.Count(); i++)
        {
            var dtoLocation = dto.Locations.ElementAt(i);
            var modelLocation = model.Locations.ElementAt(i);
            dtoLocation.ShouldBeSameAs(modelLocation);
        }
    }
}