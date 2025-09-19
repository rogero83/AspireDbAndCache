using AspireDbAndCache.Api.Interfaces;
using ZiggyCreatures.Caching.Fusion;

namespace AspireDbAndCache.Api.Services
{
    public class CacheService(
        IFusionCache cache
        )
        : ICacheService
    {
        public async Task<TValue> GetOrSetAsync<TValue>(string key,
            Func<CancellationToken, Task<TValue>> factory,
            IEnumerable<string>? tags,
            CancellationToken ct)
        {
            return await cache.GetOrSetAsync<TValue>(key, (_, ct) => factory(ct),
                tags: tags,
                token: ct);
        }

        public async Task RemoveAsync(string key, CancellationToken ct = default)
        {
            await cache.RemoveAsync(key, token: ct);
        }

        public async Task RemoveByTagAsync(string tag, CancellationToken ct)
        {
            await cache.RemoveByTagAsync(tag, token: ct);
        }
    }
}
