using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.UpdateOrganisation;

public sealed class UpdateOrganisationRequestValidator : Validator<UpdateOrganisationRequest>
{
    public UpdateOrganisationRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The organisation Id is required.");
        RuleFor(p => p.Name).NotEmpty().WithMessage("The organisation name is required.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("The organisation description is required.");
    }
}