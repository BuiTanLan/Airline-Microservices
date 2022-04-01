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
        private readonly IInvalidateCacheRequest<TRequest, TResponse> _invalidateCacheRequest;


        public InvalidateCachingBehavior(IEasyCachingProviderFactory cachingFactory,
            ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger,
            IInvalidateCacheRequest<TRequest, TResponse> invalidateCacheRequest)
        {
            _logger = logger;
            _cachingProvider = cachingFactory.GetCachingProvider("mem");
            _invalidateCacheRequest = invalidateCacheRequest;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_invalidateCacheRequest == null)
            {
                // No cache request found, so just continue through the pipeline
                return await next();
            }

            var cacheKey = _invalidateCacheRequest.GetCacheKey(request);
            var response = await next();

            await _cachingProvider.RemoveAsync(cacheKey);

            _logger.LogDebug("Cache data with cache key: {CacheKey} removed.", cacheKey);

            return response;
        }
    }
}
