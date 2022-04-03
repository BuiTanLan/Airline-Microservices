using System;
using System.Linq;
using MediatR;

namespace BuildingBlocks.Caching
{
    public interface IInvalidateCacheRequest<in TRequest, out TResponse>
        where TRequest : IRequest<TResponse>
    {
        string CacheKey { get; }
    }
}
