using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Handlers.Queries.ServiceSearch;

public readonly struct ServiceSearchQuery(string postcode, Distance distance, int maxResults)
{
    public string Postcode { get; } = postcode;
    public Distance Distance { get; } = distance;
    public int MaxResults { get; } = maxResults;
}