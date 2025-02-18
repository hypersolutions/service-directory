using ServiceDirectory.Application.Shared;
using ServiceDirectory.Domain.Primitives;

namespace ServiceDirectory.Application.Clients;

public interface IPostcodeClient
{
    Task<Result<Coordinate>> ResolvePostcodeLocationAsync(string postcode, CancellationToken cancellationToken);
}