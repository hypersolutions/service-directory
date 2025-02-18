using ServiceDirectory.Domain;

namespace ServiceDirectory.Api.Test.Support;

public static class TestLocations
{
    public static Location BristolCountyCouncil()
    {
        return new Location
        {
            AddressLine1 = "City Hall",
            AddressLine2 = "College Green",
            TownOrCity = "Bristol",
            County = "Bristol",
            Postcode = "BS1 5TR",
            Latitude = 51.452605,
            Longitude = -2.60207,
            Status = StatusType.Active
        };
    }
    
    public static Location SouthamptonCountyCouncil()
    {
        return new Location
        {
            AddressLine1 = "Civic Centre",
            AddressLine2 = null,
            TownOrCity = "Southampton",
            County = "Hampshire",
            Postcode = "SO14 7LY",
            Latitude = 50.907683,
            Longitude = -1.406979,
            Status = StatusType.Active
        };
    }
    
    public static Location AAMeeting()
    {
        return new Location
        {
            AddressLine1 = "19A Stretford Rd",
            AddressLine2 = null,
            TownOrCity = "Bristol",
            County = "Bristol",
            Postcode = "BS5 7AW",
            Latitude = 51.463579,
            Longitude = -2.550406,
            Status = StatusType.Active
        };
    }
    
    public static Location SpecialEducationalNeedsTeam()
    {
        return new Location
        {
            AddressLine1 = "SEND 0-25 Service",
            AddressLine2 = null,
            TownOrCity = "Southampton",
            County = "Hampshire",
            Postcode = "SO14 7LY",
            Latitude = 50.9083824,
            Longitude = -1.4069646,
            Status = StatusType.Active
        };
    }
    
    public static Location TowerHamlets()
    {
        return new Location
        {
            AddressLine1 = "Ewart Place 1",
            AddressLine2 = null,
            TownOrCity = "London",
            County = "London",
            Postcode = "E3 5EQ",
            Latitude = 51.532446,
            Longitude = -0.030239,
            Status = StatusType.Active
        };
    }
    
    public static Location EastSussexCountyCouncil()
    {
        return new Location
        {
            AddressLine1 = "County Hall",
            AddressLine2 = "St Anne's Crescent",
            TownOrCity = "Lewes",
            County = "East Sussex",
            Postcode = "BN7 1UE",
            Latitude = 50.871785,
            Longitude = 0.00089,
            Status = StatusType.Active
        };
    }
}