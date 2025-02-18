using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Test.Support;

public static class TestServices
{
    public static Service AAMeeting()
    {
        var service =  new Service
        {
            Name = "AA Meeting - Discussion Group",
            Description = "AA Meeting - Discussion Group",
            Cost = 10.00M,
            Status = StatusType.Active
        };
        
        service.AddLocation(TestLocations.AAMeeting());

        return service;
    }
    
    public static Service SpecialEducationalNeedsTeam()
    {
        var service =  new Service
        {
            Name = "0-25 Service - Special Educational Needs Team",
            Description = "AA Meeting - Discussion Group",
            Cost = 10.00M,
            Status = StatusType.Active
        };
        
        service.AddLocation(TestLocations.SpecialEducationalNeedsTeam());

        return service;
    }
}