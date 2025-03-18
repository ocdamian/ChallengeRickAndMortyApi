using ChallengeRickAndMortyApi.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ChallengeRickAndMortyApi.Infrastructure.Caching
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchFunction, int durationInSeconds = 120)
        {
            if (_cache.TryGetValue(key, out T cachedValue))
            {
                return cachedValue;
            }

            var createdValue = await fetchFunction();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(durationInSeconds));

            _cache.Set(key, createdValue, cacheEntryOptions);

            return createdValue;
        }
    }
}
