using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    /// <summary>
    /// 运行模块方法
    /// </summary>
    public class StartupModuleRunner
    {
        private readonly StartupModulesOptions _options;

        public StartupModuleRunner(StartupModulesOptions options)
        {
            _options = options;
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            var ctx = new ConfigureServicesContext(configuration, hostingEnvironment, _options);
            foreach (var cfg in _options.StartupModules)
            {
                cfg.ConfigureServices(services, ctx);
            }
        }

        public void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            using (var scope = app.ApplicationServices.CreateScope()) {
                var ctx = new ConfigureMiddlewareContext(configuration, hostingEnvironment, scope.ServiceProvider, _options);

                foreach (var cfg in _options.StartupModules)
                {
                    cfg.Configure(app, ctx);
                }
            }
               
        }

    }
}
