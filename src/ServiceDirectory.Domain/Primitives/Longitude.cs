namespace ServiceDirectory.Domain.Primitives;

public sealed record Longitude
{
    private readonly double _latitude;
    private const double MinLongitude = -90;
    private const double MaxLongitude = 90;
    
    public Longitude(double value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MinLongitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxLongitude);
        _latitude = value;
    }
    
    public static implicit operator Longitude(double value) => new(value);
    
    public static implicit operator double(Longitude cost) => cost._latitude;

    public static bool TryCreate(double value, out Longitude longitude)
    {
        longitude = 0;
        
        if (value is < MinLongitude or > MaxLongitude) return false;

        longitude = new Longitude(value);
        
        return true;
    }
    
    public override string ToString() => _latitude.ToString("0.000000");
}