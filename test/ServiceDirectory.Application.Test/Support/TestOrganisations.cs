using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Test.Support;

public static class TestOrganisations
{
    public static Organisation TowerHamlets(bool addServices = true)
    {
        var organisation = new Organisation
        {
            Id = 1,
            Name = "Tower Hamlets",
            Description = "Tower Hamlets organisation",
            Status = StatusType.Active
        };

        if (addServices)
        {
            var parentsCentreLocation = TestLocations.ParentsCentre();
            var parentsCentreService = TestServices.ParentsCentre();
            parentsCentreService.AddLocation(parentsCentreLocation);
            organisation.AddService(parentsCentreService);
        }

        return organisation;
    }
    
    public static Organisation CityOfLondon(bool addServices = true)
    {
        var organisation = new Organisation
        {
            Id = 2,
            Name = "City of London",
            Description = "City of London organisation",
            Status = StatusType.Active
        };

        if (addServices)
        {
            var southWestJohnSmithLocation = TestLocations.SouthWestJohnSmith();
            var southWestJohnSmithService = TestServices.SouthWestJohnSmith();
            southWestJohnSmithService.AddLocation(southWestJohnSmithLocation);
            organisation.AddService(southWestJohnSmithService);

            var specialistMidwifeLocation = TestLocations.SpecialistMidwife();
            var specialistMidwifeService = TestServices.SpecialistMidwife();
            specialistMidwifeService.AddLocation(specialistMidwifeLocation);
            organisation.AddService(specialistMidwifeService);
        }

        return organisation;
    }
}