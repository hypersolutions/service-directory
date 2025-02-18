using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.DeleteOrganisation;

public sealed class DeleteOrganisationRequestValidator : Validator<DeleteOrganisationRequest>
{
    public DeleteOrganisationRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The organisation Id is invalid.");
    }
}