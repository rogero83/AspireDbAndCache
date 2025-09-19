namespace AspireDbAndCache.Api.Interfaces
{
    public interface ICacheService
    {
        Task<TValue> GetOrSetAsync<TValue>(string key,
            Func<CancellationToken, Task<TValue>> factory,
            IEnumerable<string>? tags = null, CancellationToken ct = default);
        Task RemoveAsync(string v, CancellationToken ct = default);
        Task RemoveByTagAsync(string tag, CancellationToken ct = default);
    }
}
