using MediatR;

namespace BuildingBlocks.Caching;

public interface ICacheRequest<in TRequest, out TResponse>
    where TRequest : IRequest<TResponse>
{
    string CacheKey { get; }
    DateTime? AbsoluteExpirationRelativeToNow { get; }
}
