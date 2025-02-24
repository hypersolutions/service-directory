using System.Net;
using System.Web;

namespace ServiceDirectory.Api.Test.Support;

public sealed class TestPostcodeLookup(string postcode, HttpStatusCode statusCode, TestPostcodeResult? result = null)
{
    public string Postcode { get; } = HttpUtility.UrlPathEncode(postcode);
    public HttpStatusCode StatusCode { get; } = statusCode;
    public TestPostcodeResult? Result { get; } = result;
}