using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteLocation;

public sealed class DeleteLocationCommandHandler : IHandler<DeleteLocationCommand, Result<Location>>
{
    private readonly IApplicationRepository _repository;

    public DeleteLocationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Location>> HandleAsync(
        DeleteLocationCommand request, 
        CancellationToken cancellationToken)
    {
        var location = await _repository.Locations.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (location is null) return new Error($"Unable to find the location for {request.Id}.", ErrorType.NotFound);

        location.Status = StatusType.Inactive;
        await _repository.SaveChangesAsync(cancellationToken);
        
        return location;
    }
}