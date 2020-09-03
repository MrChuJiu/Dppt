using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Core.Flow.StartupModules
{
    /// <summary>
    /// 启动模块运行器
    /// </summary>
    public class StartupModuleRunner
    {
        private readonly StartupModulesOptions _options;

        /// <summary>
        ///  初始化实例 通过 StartupModulesOptions 来发现 IStartupModule
        /// </summary>
        public StartupModuleRunner(StartupModulesOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// 通过调用 IStartupModule.ConfigureServices(IServiceCollection, ConfigureServicesContext)  发现 IStartupModule
        /// </summary>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            var ctx = new ConfigureServicesContext(configuration, hostingEnvironment, _options);

            foreach (var cfg in _options.StartupModules)
            {
                cfg.ConfigureServices(services, ctx);
            }
        }

        /// <summary>
        /// 通过 IStartupModule.Configure(IApplicationBuilder, ConfigureMiddlewareContext)  发现 IStartupModule
        /// </summary>
        public void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var ctx = new ConfigureMiddlewareContext(configuration, hostingEnvironment, scope.ServiceProvider, _options);
            foreach (var cfg in _options.StartupModules)
            {
                cfg.Configure(app, ctx);
            }
        }

        /// <summary>
        /// 调用 发现 IApplicationInitializer
        /// </summary>
        /// <param name="serviceProvider">应用程序的根服务提供程序</param>
        public async Task RunApplicationInitializers(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var applicationInitializers = _options.ApplicationInitializers
                .Select(t =>
                {
                    try
                    {
                        return ActivatorUtilities.CreateInstance(scope.ServiceProvider, t);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to create instace of {nameof(IApplicationInitializer)} '{t.Name}'.", ex);
                    }
                })
                .Cast<IApplicationInitializer>();

            foreach (var initializer in applicationInitializers)
            {
                try
                {
                    await initializer.Invoke();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"An exception occured during the execution of {nameof(IApplicationInitializer)} '{initializer.GetType().Name}'.", ex);
                }
            }
        }
    }
}
