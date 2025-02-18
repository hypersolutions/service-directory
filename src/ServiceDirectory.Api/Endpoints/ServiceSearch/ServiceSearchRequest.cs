// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace ServiceDirectory.Api.Endpoints.ServiceSearch;

public sealed class ServiceSearchRequest
{
    public required string Postcode { get; init; }
    public int Distance { get; init; } = 10;
    public int MaxResults { get; init; } = 10;
}