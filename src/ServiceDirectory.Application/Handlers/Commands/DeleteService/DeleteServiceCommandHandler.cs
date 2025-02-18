using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Commands.DeleteService;

public sealed class DeleteServiceCommandHandler : IHandler<DeleteServiceCommand, Result<Service>>
{
    private readonly IApplicationRepository _repository;

    public DeleteServiceCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Service>> HandleAsync(
        DeleteServiceCommand request, 
        CancellationToken cancellationToken)
    {
        var service = await _repository.Services.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (service is null) return new Error($"Unable to find the service for {request.Id}.", ErrorType.NotFound);

        service.Status = StatusType.Inactive;
        await _repository.SaveChangesAsync(cancellationToken);
        
        return service;
    }
}