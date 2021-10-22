using System;
using System.Collections.Generic;
using System.Text;
using Easy.Core.Flow.Caching.Configuration;
using Easy.Core.Flow.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.Caching
{
    public static class EasyCoreFlowCachingServiceCollectionExtensions
    {
        public static IServiceCollection AddEasyCoreFlowCaching(this IServiceCollection services, Func<CachingConfiguration, CachingConfiguration> configurationAction)
        {
            services.AddSingleton<ICachingConfiguration>(configurationAction.Invoke(new CachingConfiguration()));
            services.AddSingleton<ICacheManager, DefaultMemoryCacheManager>();

            return services;
        }
    }
}
