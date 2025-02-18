namespace ServiceDirectory.Domain.Primitives;

public sealed record Latitude
{
    private readonly double _latitude;
    private const double MinLatitude = -90;
    private const double MaxLatitude = 90;
    
    public Latitude(double value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MinLatitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxLatitude);
        _latitude = value;
    }
    
    public static implicit operator Latitude(double value) => new(value);
    
    public static implicit operator double(Latitude cost) => cost._latitude;

    public static bool TryCreate(double value, out Latitude latitude)
    {
        latitude = 0;
        
        if (value is < MinLatitude or > MaxLatitude) return false;

        latitude = new Latitude(value);
        
        return true;
    }
    
    public override string ToString() => _latitude.ToString("0.000000");
}