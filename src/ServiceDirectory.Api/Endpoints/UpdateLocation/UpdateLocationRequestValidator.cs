using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.UpdateLocation;

public sealed class UpdateLocationRequestValidator : Validator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The location Id is invalid.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The service address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The service town is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The service county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The service postcode is required.");
    }
}