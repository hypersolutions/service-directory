using System.Net;
using FastEndpoints;
using Shouldly;

namespace ServiceDirectory.Api.Test.Support;

public static class ErrorResponseExtensions
{
    public static void ShouldContainError(this ErrorResponse error, string message)
    {
        error.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        error.Errors.SelectMany(e => e.Value).Any(v => v == message).ShouldBeTrue();
    }
}