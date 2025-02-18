using NSubstitute;

namespace ServiceDirectory.Application.Test;

public abstract class TestBase<TSubject>
{
    private readonly Dictionary<Type, object> _mocks = new();
        
    protected TestBase()
    {
        _mocks.Clear();

        var ctor = typeof(TSubject).GetConstructors().First();

        foreach (var ctorParam in ctor.GetParameters())
        {
            _mocks.Add(ctorParam.ParameterType, Substitute.For([ctorParam.ParameterType], []));
        }

        Subject = (TSubject)ctor.Invoke(_mocks.Values.ToArray());
    }

    protected TSubject Subject { get; }

    protected TInterface MockFor<TInterface>() where TInterface : class
    {
        if (_mocks.ContainsKey(typeof(TInterface)))
            return (TInterface)_mocks[typeof(TInterface)];

        throw new InvalidOperationException("Cannot find mock for type: " + typeof(TInterface).FullName);
    }
}