namespace ServiceDirectory.Domain.Primitives;

public sealed record Cost
{
    private readonly decimal _cost;
    private const decimal MinCost = 0M;
    private const decimal MaxCost = 2000M;

    public Cost(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MinCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MaxCost);
        _cost = value;
    }
    
    public static readonly Cost Free = new(0M);
    
    public static implicit operator Cost(decimal value) => new(value);
    
    public static implicit operator decimal(Cost cost) => cost._cost;

    public static bool TryCreate(decimal value, out Cost cost)
    {
        cost = Free;
        
        if (value is < MinCost or > MaxCost) return false;

        cost = new Cost(value);
        
        return true;
    }
    
    public override string ToString() => _cost.ToString("0.00");
}