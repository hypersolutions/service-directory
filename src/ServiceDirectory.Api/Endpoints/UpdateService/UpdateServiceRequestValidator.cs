using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.UpdateService;

public sealed class UpdateServiceRequestValidator : Validator<UpdateServiceRequest>
{
    public UpdateServiceRequestValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0).WithMessage("The service Id is required.");
        RuleFor(p => p.Name).NotEmpty().WithMessage("The service name is required.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("The service description is required.");
        RuleFor(p => p.Cost).InclusiveBetween(0, 2000).WithMessage("The service cost must be between 0 (free) and £2,000.");
    }
}