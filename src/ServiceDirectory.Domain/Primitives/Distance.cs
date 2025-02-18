namespace ServiceDirectory.Domain.Primitives;

public sealed record Distance
{
    private readonly double _kilometres;
    public const int KilometresToMetres = 1000;
    private const double MinKilometres = 0.0;
    private const double MaxKilometres = 30.0;
    
    public Distance(double kilometres)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(kilometres, MinKilometres);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(kilometres, MaxKilometres);
        _kilometres = kilometres;
    }

    public double AsMetres() => _kilometres * KilometresToMetres;
    
    public static implicit operator Distance(double value) => new(value);
    
    public static implicit operator double(Distance cost) => cost._kilometres;

    public static bool TryCreate(double value, out Distance distance)
    {
        distance = MinKilometres;
        
        if (value is < MinKilometres or > MaxKilometres) return false;

        distance = new Distance(value);
        
        return true;
    }
    
    public override string ToString() => _kilometres.ToString("0.00");
}