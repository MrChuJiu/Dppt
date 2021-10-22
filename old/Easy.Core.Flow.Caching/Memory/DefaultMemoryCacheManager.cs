using Easy.Core.Flow.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.Caching.Memory
{
    public class DefaultMemoryCacheManager : CacheManagerBase
    {
        public DefaultMemoryCacheManager(ICachingConfiguration configuration) : base(configuration)
        {
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new DefaultMemoryCache(name);
        }

        protected override void DisposeCaches()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
        }
    }
}
