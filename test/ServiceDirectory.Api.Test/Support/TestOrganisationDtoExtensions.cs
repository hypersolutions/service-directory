using ServiceDirectory.Api.Dtos;
using ServiceDirectory.Domain;
using Shouldly;

namespace ServiceDirectory.Api.Test.Support;

public static class TestOrganisationDtoExtensions
{
    public static void ShouldBeSameAs(this List<OrganisationDto> dtoList, params Organisation[] models)
    {
        dtoList.Count.ShouldBe(models.Length);

        for (var i = 0; i < dtoList.Count; i++)
        {
            dtoList[i].ShouldBeSameAs(models[i]);
        }
    }
    
    public static void ShouldBeSameAs(this OrganisationDto dto, Organisation model, bool checkServices = true)
    {
        dto.Id.ShouldBeGreaterThan(0);
        dto.Name.ShouldBe(model.Name);
        dto.Description.ShouldBe(model.Description);
        dto.Locations.Count().ShouldBe(model.Locations.Count());

        for (var i = 0; i < dto.Locations.Count(); i++)
        {
            var dtoLocation = dto.Locations.ElementAt(i);
            var modelLocation = model.Locations.ElementAt(i);
            dtoLocation.ShouldBeSameAs(modelLocation);
        }

        if (checkServices)
        {
            dto.Services.Count().ShouldBe(model.Services.Count());

            for (var i = 0; i < dto.Services.Count(); i++)
            {
                var dtoService = dto.Services.ElementAt(i);
                var modelService = model.Services.ElementAt(i);
                dtoService.ShouldBeSameAs(modelService);
            }
        }
    }
}