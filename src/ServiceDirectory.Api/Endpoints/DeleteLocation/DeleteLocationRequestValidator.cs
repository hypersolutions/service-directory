using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.DeleteLocation;

public sealed class DeleteLocationRequestValidator : Validator<DeleteLocationRequest>
{
    public DeleteLocationRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The location Id is invalid.");
    }
}