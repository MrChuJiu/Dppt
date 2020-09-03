using Microsoft.AspNetCore.Hosting;
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
            return UseStartupModules(builder, options => options.DiscoverStartupModules());
        }
        /// <summary>
        /// 配置具有指定配置的启动模块
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseStartupModules(this IWebHostBuilder builder, Action<StartupModulesOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new StartupModulesOptions();
            configure(options);

            if (options.StartupModules.Count == 0)
            {
                // 这里没什么可做的
                return builder;
            }

            var runner = new StartupModuleRunner(options);
            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IStartupFilter>(sp => ActivatorUtilities.CreateInstance<ModulesStartupFilter>(sp, runner));
                runner.ConfigureServices(services, hostContext.Configuration, hostContext.HostingEnvironment);
            });

            return builder;
        }

    }
}
