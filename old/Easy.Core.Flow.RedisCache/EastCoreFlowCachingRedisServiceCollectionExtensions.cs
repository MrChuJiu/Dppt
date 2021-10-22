using System;
using System.Collections.Generic;
using System.Text;
using Easy.Core.Flow.Caching;
using Easy.Core.Flow.Caching.Configuration;
using Easy.Core.Flow.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.RedisCache
{
    public static class EastCoreFlowCachingRedisServiceCollectionExtensions
    {
        public static IServiceCollection AddEasyCoreFlowRedis(this IServiceCollection services, Func<CachingConfiguration, CachingConfiguration> configurationAction)
        {
            services.AddSingleton<ICachingConfiguration>(configurationAction.Invoke(new CachingConfiguration()));

            services.AddSingleton<RedisCacheOptions>();
            services.AddTransient<IRedisCacheDatabaseProvider, RedisCacheDatabaseProvider>();
            services.AddTransient<IRedisCacheSerializer, DefaultRedisCacheSerializer>();
            services.AddSingleton<ICacheManager, RedisCacheManager>();

            return services;
        }
    }
}
