using FastEndpoints;
using FluentValidation;

namespace ServiceDirectory.Api.Endpoints.ServiceSearch;

public class ServiceSearchRequestValidator : Validator<ServiceSearchRequest>
{
    public ServiceSearchRequestValidator()
    {
        RuleFor(p => p.Postcode).NotEmpty().WithMessage("The postcode is required.");
        RuleFor(p => p.Distance).InclusiveBetween(1, 30).WithMessage("The distance must be between 1 and 30 km inclusive.");
        RuleFor(p => p.MaxResults).InclusiveBetween(1, 10).WithMessage("The max results must be between 1 and 10 inclusive.");
    }
}