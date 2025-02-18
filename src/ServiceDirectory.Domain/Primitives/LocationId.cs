namespace ServiceDirectory.Domain.Primitives;

public sealed record LocationId
{
    private readonly int _value;

    public LocationId(int value)
    {
        _value = value;
    }
    
    public static implicit operator LocationId(int value) => new(value);

    public static implicit operator int(LocationId id) => id._value;
    
    public override string ToString() => _value.ToString();
}