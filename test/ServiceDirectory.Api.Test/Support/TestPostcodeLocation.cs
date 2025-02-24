namespace ServiceDirectory.Api.Test.Support;

public sealed class TestPostcodeLocation(TestPostcodeResult result)
{
    public TestPostcodeResult Result { get; init; } = result;
}