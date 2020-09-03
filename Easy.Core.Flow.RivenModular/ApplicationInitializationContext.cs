using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public class ApplicationInitializationContext
    {
        public IServiceProvider ServiceProvider { get; }

        public IConfiguration Configuration { get; }

        public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider, [NotNull] IConfiguration configuration)
        {
            ServiceProvider = serviceProvider;
            Configuration = configuration;
        }
    }
}
