namespace ServiceDirectory.Domain.Primitives;

public sealed record ServiceId
{
    private readonly int _value;

    public ServiceId(int value)
    {
        _value = value;
    }
    
    public static implicit operator ServiceId(int value) => new(value);
    
    public static implicit operator int(ServiceId id) => id._value;
    
    public override string ToString() => _value.ToString();
}