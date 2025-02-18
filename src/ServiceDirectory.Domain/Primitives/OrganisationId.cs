namespace ServiceDirectory.Domain.Primitives;

public sealed record OrganisationId
{
    private readonly int _value;

    public OrganisationId(int value)
    {
        _value = value;
    }
    
    public static implicit operator OrganisationId(int value) => new(value);
    
    public static implicit operator int(OrganisationId id) => id._value;

    public override string ToString() => _value.ToString();
}