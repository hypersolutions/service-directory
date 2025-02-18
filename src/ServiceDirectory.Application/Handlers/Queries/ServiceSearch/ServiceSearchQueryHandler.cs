using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Queries.ServiceSearch;

public sealed class ServiceSearchQueryHandler : IHandler<ServiceSearchQuery, Result<IEnumerable<ServiceDetail>>>
{
    private readonly IPostcodeClient _postcodeClient;
    private readonly IApplicationRepository _repository;

    public ServiceSearchQueryHandler(IPostcodeClient postcodeClient, IApplicationRepository repository)
    {
        _postcodeClient = postcodeClient;
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ServiceDetail>>> HandleAsync(ServiceSearchQuery request, CancellationToken cancellationToken)
    {
        var result = await _postcodeClient.ResolvePostcodeLocationAsync(request.Postcode, cancellationToken);

        if (result.IsError) return (Error)result!;

        return SearchAsync(result!, request.Distance, request.MaxResults);
    }
    
    private Result<IEnumerable<ServiceDetail>> SearchAsync(Coordinate searchCoordinate, Distance distance, int take)
    {
        const double degreesToRadians = Math.PI / 180;
        const double earthRadiusInMetres = 6378100;
        
        return (
            from o in _repository.Organisations.Include(o => o.Services).Include(s => s.Locations)
            from s in o.Services
            from l in s.Locations
            let d = 2 * earthRadiusInMetres * Math.Asin(
                Math.Sqrt(Math.Pow(Math.Sin((searchCoordinate.Latitude - l.Latitude) * degreesToRadians / 2), 2) +
                          Math.Cos(l.Latitude * degreesToRadians) *
                          Math.Cos(searchCoordinate.Latitude * degreesToRadians) *
                          Math.Pow(Math.Sin((searchCoordinate.Longitude - l.Longitude) * degreesToRadians / 2), 2)))
            where d < distance.AsMetres()
            orderby d
            select new ServiceDetail(o, s, l, new Distance(d / Distance.KilometresToMetres))).Take(take).ToList();
    }

    // The above is based upon the haversine algorithm. The example below is from: https://www.movable-type.co.uk/scripts/latlong.html
    
    // It is inlined to enable it to work with EF and linq
    
    // private static double Haversine(double lat1, double lat2, double lon1, double lon2)
    // {
    //     const double r = 6378100; // meters
    //     var sdlat = Math.Sin((lat2 - lat1) / 2);
    //     var sdlon = Math.Sin((lon2 - lon1) / 2);
    //     var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
    //     var d = 2 * r * Math.Asin(Math.Sqrt(q));
    //     return d;
    // }
}