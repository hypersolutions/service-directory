using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateOrganisation;

public sealed class UpdateOrganisationCommandHandler : IHandler<UpdateOrganisationCommand, Result<Organisation>>
{
    private readonly IApplicationRepository _repository;

    public UpdateOrganisationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Organisation>> HandleAsync(
        UpdateOrganisationCommand request, 
        CancellationToken cancellationToken)
    {
        var organisation = await _repository.Organisations.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (organisation is null) return new Error($"Unable to find the organisation for {request.Id}.", ErrorType.NotFound);
        
        organisation.Name = request.Name;
        organisation.Description = request.Description;
        await _repository.SaveChangesAsync(cancellationToken);
        
        return organisation;
    }
}