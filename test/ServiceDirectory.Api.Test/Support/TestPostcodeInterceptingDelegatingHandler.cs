using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;

namespace ServiceDirectory.Api.Test.Support;

public sealed class TestPostcodeInterceptingDelegatingHandler : DelegatingHandler
{
    private readonly List<TestPostcodeLookup> _testPostcodeLooks;

    public TestPostcodeInterceptingDelegatingHandler(List<TestPostcodeLookup> testPostcodeLooks)
    {
        _testPostcodeLooks = testPostcodeLooks;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var postcodeLookup = _testPostcodeLooks.FirstOrDefault(
            pl => request.RequestUri?.AbsoluteUri.EndsWith(HttpUtility.UrlPathEncode(pl.Postcode)) == true);

        if (postcodeLookup is null)
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        
        var response = new HttpResponseMessage(postcodeLookup.StatusCode);

        if (postcodeLookup.Result is not null)
        {
            response.Content = new StringContent(
                JsonSerializer.Serialize(
                    new TestPostcodeLocation(postcodeLookup.Result)), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        return Task.FromResult(response);
    }
}