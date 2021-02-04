using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Easy.Core.Flow.Caching.Configuration;

namespace Easy.Core.Flow.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ICachingConfiguration Configuration;
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase(ICachingConfiguration configuration)
        {
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        public ICache GetCache(string name)
        {
            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(name);

                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);
                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }
                return cache;
            });
        }
        public void Dispose()
        {
            Caches.Clear();
        }

        protected abstract void DisposeCaches();
        /// <summary>
        /// 用于创建实际的缓存实现
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected abstract ICache CreateCacheImplementation(string name);

    }
}
