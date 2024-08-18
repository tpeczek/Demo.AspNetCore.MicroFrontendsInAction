using EsiNet.Caching;

namespace Demo.AspNetCore.MicroFrontendsInAction.Proxy.Caching
{
    internal class EsiNoOpFragmentCache : IEsiFragmentCache
    {
        public Task Set<T>(CacheKey key, T value, TimeSpan absoluteExpirationRelativeToNow) => Task.CompletedTask;

        public Task<(bool, T)> TryGet<T>(CacheKey key) => Task.FromResult((false, default(T)));
    }
}
