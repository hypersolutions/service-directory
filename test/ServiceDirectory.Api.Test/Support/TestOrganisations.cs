using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Test.Support;

public static class TestOrganisations
{
    public static Organisation BristolCountyCouncil()
    {
        var organisation = new Organisation
        {
            Name = "Bristol County Council",
            Description = "Bristol County Council",
            Status = StatusType.Active
        };
        
        organisation.AddLocation(TestLocations.BristolCountyCouncil());
        organisation.AddService(TestServices.AAMeeting());

        return organisation;
    }
    
    public static Organisation SouthamptonCountyCouncil()
    {
        var organisation = new Organisation
        {
            Name = "Southampton County Council",
            Description = "Southampton County Council",
            Status = StatusType.Active
        };
        
        organisation.AddLocation(TestLocations.SouthamptonCountyCouncil());
        organisation.AddService(TestServices.SpecialEducationalNeedsTeam());

        return organisation;
    }
    
    public static Organisation TowerHamlets()
    {
        var organisation = new Organisation
        {
            Name = "Tower Hamlets",
            Description = "Tower Hamlets",
            Status = StatusType.Active
        };
        
        organisation.AddLocation(TestLocations.TowerHamlets());

        return organisation;
    }
    
    public static Organisation EastSussexCountyCouncil()
    {
        var organisation = new Organisation
        {
            Name = "East Sussex County Council",
            Description = "East Sussex County Council",
            Status = StatusType.Active
        };
        
        organisation.AddLocation(TestLocations.TowerHamlets());

        return organisation;
    }
}