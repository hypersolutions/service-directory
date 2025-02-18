using Microsoft.EntityFrameworkCore;
using ServiceDirectory.Application.Data;
using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain;

namespace ServiceDirectory.Application.Handlers.Commands.UpdateService;

public sealed class UpdateServiceCommandHandler : IHandler<UpdateServiceCommand, Result<Service>>
{
    private readonly IApplicationRepository _repository;

    public UpdateServiceCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<Service>> HandleAsync(
        UpdateServiceCommand request, 
        CancellationToken cancellationToken)
    {
        var service = await _repository.Services.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (service is null) return new Error($"Unable to find the service for {request.Id}.", ErrorType.NotFound);
        
        service.Name = request.Name;
        service.Description = request.Description;
        service.Cost = request.Cost;
        await _repository.SaveChangesAsync(cancellationToken);
        
        return service;
    }
}