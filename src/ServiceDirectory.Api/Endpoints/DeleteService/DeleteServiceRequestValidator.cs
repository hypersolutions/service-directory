using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.DeleteService;

public sealed class DeleteServiceRequestValidator : Validator<DeleteServiceRequest>
{
    public DeleteServiceRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The service Id is invalid.");
    }
}