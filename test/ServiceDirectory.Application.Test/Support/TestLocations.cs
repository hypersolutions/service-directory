using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Test.Support;

public static class TestLocations
{
    public static Location ParentsCentre() => new()
    {
        Id = 1,
        AddressLine1 = "Tower Hamlets Parents Centre",
        AddressLine2 = "1 Links Yard",
        TownOrCity = "London",
        County = "London",
        Postcode = "E1 5LX",
        Latitude = 51.519437,
        Longitude = 0.06992
    };
    
    public static Location DiscoveryHome() => new()
    {
        Id = 2,
        AddressLine1 = "The Quab Break Service Ltd",
        AddressLine2 = "31 - 33 Spelman Street",
        TownOrCity = "London",
        County = "London",
        Postcode = "E1 5LQ",
        Latitude = 51.518578,
        Longitude = 0.06895
    };
    
    public static Location SouthWestJohnSmith() => new()
    {
        Id = 3,
        AddressLine1 = "John Smith Children And Family Centre",
        AddressLine2 = "90 Stepney Way",
        TownOrCity = "London",
        County = "London",
        Postcode = "E1 2EN",
        Latitude = 51.517612,
        Longitude = 0.056838
    };
    
    public static Location SpecialistMidwife() => new()
    {
        Id = 4,
        AddressLine1 = "The Royal London Hospital",
        AddressLine2 = "Whitechapel Road",
        TownOrCity = "London",
        County = "London",
        Postcode = "E1 1BB",
        Latitude = 51.519019,
        Longitude = -0.058106
    };
}