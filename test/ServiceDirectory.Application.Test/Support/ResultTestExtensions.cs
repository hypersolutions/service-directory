using ServiceDirectory.Application.Shared;
using Shouldly;

namespace ServiceDirectory.Application.Test.Support;

public static class ResultTestExtensions
{
    public static async Task ShouldBeSuccessMatchAsync<T>(this Result<T> result, Func<T, bool> matchWith) where T : class
    {
        var value = await result.MatchAsync<T?>(
            async x => matchWith(x) ? await Task.FromResult(x) : await Task.FromResult<T?>(null), 
            async _ => await Task.FromResult<T?>(null));
        value.ShouldNotBeNull();
    }
    
    public static void ShouldBeSuccessMatch<T>(this Result<T> result, Func<T, bool> matchWith) where T : class
    {
        var value = result.Match<T?>(x => matchWith(x) ? x : null, _ => null);
        value.ShouldNotBeNull();
    }
    
    public static async Task ShouldBeErrorMatchAsync<T>(this Result<T> result, Func<Error, bool> matchWith) where T : class
    {
        result.IsError.ShouldBeTrue();
        var value = await result.MatchAsync(async _ => await Task.FromResult(false), x => Task.FromResult(matchWith(x)));
        value.ShouldBeTrue();
    }
    
    public static void ShouldBeErrorMatch<T>(this Result<T> result, Func<Error, bool> matchWith) where T : class
    {
        result.IsError.ShouldBeTrue();
        var value = result.Match(_ => false, matchWith);
        value.ShouldBeTrue();
    }
}