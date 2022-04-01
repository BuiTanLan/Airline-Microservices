using MediatR;

namespace BuildingBlocks.Caching;

public interface ICacheRequest<in TRequest, out TResponse>
    where TRequest : IRequest<TResponse>
{
    string GetCacheKey(TRequest request) => $"{request.GetType().FullName}-{request.GetHashCode()}";
    public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
}
