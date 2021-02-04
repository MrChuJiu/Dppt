using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Easy.Core.Flow.Caching.Configuration
{
    public class CachingConfiguration : ICachingConfiguration
    {

        public CachingConfiguration()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        private readonly List<ICacheConfigurator> _configurators;
        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }
    }
}
