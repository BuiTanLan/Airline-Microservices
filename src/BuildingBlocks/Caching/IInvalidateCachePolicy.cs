using System;
using System.Linq;
using MediatR;

namespace BuildingBlocks.Caching
{
    public interface IInvalidateCacheRequest<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        string GetCacheKey(TRequest request) => $"{request.GetType().FullName}-{request.GetHashCode()}";
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddHours(1);
    }
}




