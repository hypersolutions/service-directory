using System.Text.Json;
using ServiceDirectory.Application.Clients;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain.Primitives;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace ServiceDirectory.Infrastructure.Clients;

public sealed class PostcodeClient : IPostcodeClient
{
    private readonly HttpClient _client;

    public PostcodeClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Result<Coordinate>> ResolvePostcodeLocationAsync(string postcode, CancellationToken cancellationToken)
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, postcode), cancellationToken);

        if (!response.IsSuccessStatusCode)
            return new Error($"Location for postcode {postcode} not unknown.", ErrorType.NotFound);

        var contents = await response.Content.ReadAsStringAsync(cancellationToken);

        var postcodeLocation = JsonSerializer.Deserialize<PostcodeLocation>(contents, JsonSerializerOptions.Web)!;

        return new Coordinate(postcodeLocation.Result.Latitude, postcodeLocation.Result.Longitude);
    }

    private sealed class PostcodeLocation
    {
        public Result Result { get; init; } = null!;
    }

    private sealed class Result
    {
        public double Latitude { get; init; } = 0;
        public double Longitude { get; init; } = 0;
    }
}