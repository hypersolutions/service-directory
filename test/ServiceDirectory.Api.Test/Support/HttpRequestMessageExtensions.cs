using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ServiceDirectory.Api.Test.Support;

public static class HttpRequestMessageExtensions
{
    public static void AddContent<TObject>(this HttpRequestMessage request, TObject instance) where TObject : class
    {
        request.Content = new StringContent(
            JsonSerializer.Serialize(instance), 
            Encoding.UTF8, 
            MediaTypeNames.Application.Json);
    }
}