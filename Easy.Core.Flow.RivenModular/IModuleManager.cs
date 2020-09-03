using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    /// <summary>
    /// 应用模块管理器
    /// </summary>
    public interface IModuleManager : IDisposable
    {
        /// <summary>
        /// 启动模块
        /// </summary>
        void StartModule<TModule>(IServiceCollection services) where TModule : IAppModule;

        /// <summary>
        /// 模块排序
        /// </summary>
        /// <typeparam name="TModule">启动模块类型</typeparam>
        /// <returns>排序结果</returns>
        List<ModuleDescriptor> ModuleSort<TModule>() where TModule : IAppModule;

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IServiceCollection ConfigurationService(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// 配置应用初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        IServiceProvider ApplicationInitialization(IServiceProvider serviceProvider);

        /// <summary>
        /// 应用程序停止
        /// </summary>
        void ApplicationShutdown();
    }
}
