using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    public class ConfigureServicesContext
    {
        public ConfigureServicesContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, StartupModulesOptions options)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            Options = options;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        public StartupModulesOptions Options { get; }
    }
}
