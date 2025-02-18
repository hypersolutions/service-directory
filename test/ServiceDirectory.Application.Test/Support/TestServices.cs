using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Test.Support;

public static class TestServices
{
    public static Service ParentsCentre() => new()
    {
        Id = 1,
        Name = "Tower Hamlets Parents Centre",
        Description = "Tower Hamlets Parents Centre",
        Cost = 10.00M
    };
    
    public static Service DiscoveryHome() => new()
    {
        Id = 2,
        Name = "Tower Hamlets Discovery Home",
        Description = "Tower Hamlets Discovery Home",
        Cost = 15.00M
    };
    
    public static Service SouthWestJohnSmith() => new()
    {
        Id = 3,
        Name = "City of London South West John Smith",
        Description = "City of London South West John Smith",
        Cost = 8.00M
    };
    
    public static Service SpecialistMidwife() => new()
    {
        Id = 4,
        Name = "City of London Specialist Midwife",
        Description = "City of London Specialist Midwife",
        Cost = Cost.Free
    };
}