using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.CreateLocation;

public sealed class CreateLocationRequestValidator : Validator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(p => new { p.ServiceId, p.OrganisationId }).Must(
                p => ValidateServiceAndOrganisationIds(p.ServiceId, p.OrganisationId))
            .WithMessage("Either service Id or organisation Id is required.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The town or city is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The postcode is required.");
    }

    private static bool ValidateServiceAndOrganisationIds(int? serviceId, int? organisationId)
        => serviceId > 0 && organisationId is null || organisationId > 0 && serviceId is null;
}