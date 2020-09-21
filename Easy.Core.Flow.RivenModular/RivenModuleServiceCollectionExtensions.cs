using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    /// <summary>
    /// 模块服务扩展
    /// </summary>
    public static class RivenModuleServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Riven模块服务
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRivenModule<TModule>(this IServiceCollection services, IConfiguration configuration)
            where TModule : IAppModule
        {
            var moduleManager = new ModuleManager();
            // 将模块都查询排序好
            moduleManager.StartModule<TModule>(services);
            // 调用模块 和 子模块的ConfigurationService方法
            moduleManager.ConfigurationService(services, configuration);
            // 注入全局的  IModuleManager
            services.TryAddSingleton<IModuleManager>(moduleManager);
            return services;
        }
    }
}
