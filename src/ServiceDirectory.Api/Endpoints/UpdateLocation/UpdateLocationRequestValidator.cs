using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.UpdateLocation;

public sealed class UpdateLocationRequestValidator : Validator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The location Id is required.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The location address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The location town or city is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The location county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The location postcode is required.");
    }
}