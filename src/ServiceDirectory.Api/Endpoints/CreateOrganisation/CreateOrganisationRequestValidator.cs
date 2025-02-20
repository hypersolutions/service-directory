using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.CreateOrganisation;

public sealed class CreateOrganisationRequestValidator : Validator<CreateOrganisationRequest>
{
    public CreateOrganisationRequestValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("The organisation name is required.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("The organisation description is required.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The organisation address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The organisation town or city is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The organisation county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The organisation postcode is required.");
    }
}