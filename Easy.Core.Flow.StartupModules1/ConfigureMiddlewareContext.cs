using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    public class ConfigureMiddlewareContext
    {
        public ConfigureMiddlewareContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IServiceProvider serviceProvider, StartupModulesOptions options)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            ServiceProvider = serviceProvider;
            Options = options;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }
        public IServiceProvider ServiceProvider { get; }

        public StartupModulesOptions Options { get; }
    }
}
