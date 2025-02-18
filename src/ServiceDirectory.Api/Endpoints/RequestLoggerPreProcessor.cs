using FastEndpoints;
// ReSharper disable ClassNeverInstantiated.Global

namespace ServiceDirectory.Api.Endpoints;

public sealed class RequestLoggerPreProcessor<TRequest> : IPreProcessor<TRequest>
{
    public Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken cancellationToken)
    {
        var logger = context.HttpContext.Resolve<ILogger<TRequest>>();

        logger.LogInformation(
            "The request {FullName} on path {Path} has been made.", 
            context.Request!.GetType().FullName, 
            context.HttpContext.Request.Path);

        return Task.CompletedTask;
    }
}