namespace ServiceDirectory.Api.Test.Support;

public sealed class TestPostcodeResult(double latitude, double longitude)
{
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;
}