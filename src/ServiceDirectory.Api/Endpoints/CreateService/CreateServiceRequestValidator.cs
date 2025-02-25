﻿using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.CreateService;

public sealed class CreateServiceRequestValidator : Validator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(p => p.OrganisationId).GreaterThan(0).WithMessage("The organisation Id is required.");
        RuleFor(p => p.Name).NotEmpty().WithMessage("The service name is required.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("The service description is required.");
        RuleFor(p => p.Cost).InclusiveBetween(0, 2000).WithMessage("The service cost must be between 0 (free) and £2,000.");
        RuleFor(p => p.AddressLine1).NotEmpty().WithMessage("The service address line 1 is required.");
        RuleFor(p => p.TownOrCity).NotEmpty().WithMessage("The service town or city is required.");
        RuleFor(p => p.County).NotEmpty().WithMessage("The service county is required.");
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The service postcode is required.");
    }
}