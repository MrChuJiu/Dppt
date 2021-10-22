using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new StartupModulesOptions();
            options.DiscoverStartupModules();

            if (options.StartupModules.Count == 0)
            {
                return builder;
            }

            var runner = new StartupModuleRunner(options);
            builder.ConfigureServices((hostContext, services) =>
            {
                
                // 注册 IStartupFilter 实现
                services.AddSingleton<IStartupFilter>(sp => ActivatorUtilities.CreateInstance<ModulesStartupFilter>(sp, runner));

                runner.ConfigureServices(services, hostContext.Configuration, hostContext.HostingEnvironment);
            });

            return builder;
        }
    }
}
