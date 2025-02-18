using System.Text.Json;
using Shouldly;

namespace ServiceDirectory.Api.Test.Support;

public static class HttpResponseMessageExtensions
{
    public static async Task<TObject> HttpResponseMessageAsync<TObject>(this HttpResponseMessage response) where TObject : class
    {
        var contents = await response.Content.ReadAsStreamAsync();
        var instance = await JsonSerializer.DeserializeAsync<TObject>(contents, JsonSerializerOptions.Web);
        instance.ShouldNotBeNull();
        return instance;
    }
}