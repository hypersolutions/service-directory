using FastEndpoints;
using FluentValidation.Results;
using ServiceDirectory.Application.Shared;

namespace ServiceDirectory.Api.Endpoints;

public abstract class EndpointBase : EndpointWithoutRequest
{
    protected Task SendHandlerErrorAsync(Error error, CancellationToken cancellationToken)
    {
        switch (error.Type)
        {
            case ErrorType.NotFound:
                return SendNoContentAsync(cancellationToken);
            case ErrorType.Conflict:
                return SendAsync(null!, StatusCodes.Status409Conflict, cancellationToken);
            case ErrorType.Unexpected:
            default:
                ValidationFailures.Add(new ValidationFailure("HandlerError", error.Description));
                return SendErrorsAsync(cancellation: cancellationToken);
        }
    }
}

public abstract class EndpointBase<TRequest> : Endpoint<TRequest> where TRequest : notnull
{
    protected Task SendHandlerErrorAsync(Error error, CancellationToken cancellationToken)
    {
        switch (error.Type)
        {
            case ErrorType.NotFound:
                return SendNoContentAsync(cancellationToken);
            case ErrorType.Conflict:
                return SendAsync(null!, StatusCodes.Status409Conflict, cancellationToken);
            case ErrorType.Unexpected:
            default:
                ValidationFailures.Add(new ValidationFailure("HandlerError", error.Description));
                return SendErrorsAsync(cancellation: cancellationToken);
        }
    }
}

public abstract class EndpointBase<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull
{
    protected Task SendHandlerErrorAsync(Error error, CancellationToken cancellationToken)
    {
        switch (error.Type)
        {
            case ErrorType.NotFound:
                return SendNoContentAsync(cancellationToken);
            case ErrorType.Conflict:
                return SendAsync(default!, StatusCodes.Status409Conflict, cancellationToken);
            case ErrorType.Unexpected:
            default:
                ValidationFailures.Add(new ValidationFailure("HandlerError", error.Description));
                return SendErrorsAsync(cancellation: cancellationToken);
        }
    }

    protected Task SendCreatedAsync(TResponse response, CancellationToken cancellationToken)
    {
        return SendAsync(response, StatusCodes.Status201Created, cancellationToken);
    }
}