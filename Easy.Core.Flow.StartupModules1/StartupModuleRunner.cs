using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules1
{
    public class StartupModuleRunner : IStartupModuleRunner
    {
        private readonly StartupModulesOptions _options;

        /// <summary>
        ///  初始化实例 通过 StartupModulesOptions 来发现 IStartupModule
        /// </summary>
        public StartupModuleRunner(StartupModulesOptions options)
        {
            _options = options;
        }
        public void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            using var scope = app.ApplicationServices.CreateScope();
            foreach (var cfg in _options.StartupModules)
            {
                cfg.Configure(app, hostingEnvironment);
            }
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            foreach (var cfg in _options.StartupModules)
            {
                cfg.ConfigureServices(services);
            }
        }
    }
}
