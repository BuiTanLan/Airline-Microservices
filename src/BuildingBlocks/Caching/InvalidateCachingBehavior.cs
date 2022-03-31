using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Caching
{
    public class InvalidateCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> _logger;
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly IEnumerable<IInvalidateCacheRequest<TRequest, TResponse>> _invalidateCacheRequests;


        public InvalidateCachingBehavior(IEasyCachingProviderFactory cachingFactory,
            ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger,
            IEnumerable<IInvalidateCacheRequest<TRequest, TResponse>> invalidateCacheRequests)
        {
            _logger = logger;
            _cachingProvider = cachingFactory.GetCachingProvider("mem");
            _invalidateCacheRequests = invalidateCacheRequests;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var cacheRequest = _invalidateCacheRequests.FirstOrDefault();
            if (cacheRequest == null)
            {
                // No cache policy found, so just continue through the pipeline
                return await next();
            }

            var cacheKey = cacheRequest.GetCacheKey(request);
            var response = await next();

            await _cachingProvider.RemoveAsync(cacheKey);

            _logger.LogDebug("Cache data with cache key: {CacheKey} removed.", cacheKey);

            return response;
        }
    }
}
