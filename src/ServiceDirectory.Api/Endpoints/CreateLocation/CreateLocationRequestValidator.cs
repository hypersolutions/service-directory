using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.CreateLocation;

public sealed class CreateLocationRequestValidator : Validator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(p => new { p.ServiceId, p.OrganisationId }).Must(p => ValidateServiceAndOrganisationIds(p.ServiceId, p.OrganisationId))
            .WithMessage("Either service Id or organisation Id is required.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The service address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The service town is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The service county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The service postcode is required.");
    }

    private static bool ValidateServiceAndOrganisationIds(int? serviceId, int? organisationId)
        => (serviceId is null && organisationId is not null) || (serviceId is not null && organisationId is null);
}