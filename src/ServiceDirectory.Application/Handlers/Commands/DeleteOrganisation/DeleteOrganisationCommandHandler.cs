using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteOrganisation;

public sealed class DeleteOrganisationCommandHandler : IHandler<DeleteOrganisationCommand, Result<Organisation>>
{
    private readonly IApplicationRepository _repository;

    public DeleteOrganisationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Organisation>> HandleAsync(
        DeleteOrganisationCommand request, 
        CancellationToken cancellationToken)
    {
        var organisation = await _repository.Organisations.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (organisation is null) return new Error($"Unable to find the organisation for {request.Id}.", ErrorType.NotFound);

        organisation.Status = StatusType.Inactive;
        await _repository.SaveChangesAsync(cancellationToken);
        
        return organisation;
    }
}