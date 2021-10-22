using System;
using System.Collections.Generic;
using System.Text;
using Easy.Core.Flow.Caching;
using Easy.Core.Flow.Caching.Configuration;
using StackExchange.Redis;

namespace Easy.Core.Flow.RedisCache
{
    public class RedisCacheManager : CacheManagerBase
    {
        // 这里设计的非常糟糕， 因为没有抽象出一个Base 来存储Di容器 
        // RedisCacheManager 构造函数的后2个参数完全你可以放到 rediscache 中 采用IocManager 的方式进行加载
        // 但是这里为了使用该库的复杂的 这样设计了
        readonly IRedisCacheDatabaseProvider RedisCacheDatabaseProvider;

        readonly IRedisCacheSerializer RedisCacheSerializer;

        public RedisCacheManager(ICachingConfiguration configuration, IRedisCacheDatabaseProvider redisCacheDatabaseProvider, IRedisCacheSerializer redisCacheSerializer) : base(configuration)
        {
            RedisCacheDatabaseProvider = redisCacheDatabaseProvider;
            RedisCacheSerializer = redisCacheSerializer;
        }

        protected override ICache CreateCacheImplementation(string name)
        {

            return new RedisCache(name).Initialize(RedisCacheDatabaseProvider, RedisCacheSerializer);
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
