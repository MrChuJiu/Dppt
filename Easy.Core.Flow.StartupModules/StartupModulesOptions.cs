using Easy.Core.Flow.StartupModules.Internal;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    /// <summary>
    /// 指定启动模块的选项
    /// </summary>
    public class StartupModulesOptions
    {
        /// <summary>
        /// 模块  StartupModules 集合 
        /// </summary>
        public ICollection<IStartupModule> StartupModules { get; } = new List<IStartupModule>();
        /// <summary>
        /// 获取用于初始化应用程序的  ApplicationInitializers 集合
        /// </summary>
        public ICollection<Type> ApplicationInitializers { get; } = new List<Type>();
        /// <summary>
        /// 获取设置
        /// </summary>
        public IDictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 搜索启动模块
        /// </summary>
        public void DiscoverStartupModules() {

            DiscoverStartupModules(Assembly.GetEntryAssembly()!);
        }

        public void DiscoverStartupModules(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0 || assemblies.All(a => a == null))
            {
                throw new ArgumentException("No assemblies to discover startup modules from specified.", nameof(assemblies));
            }

            foreach (var type in assemblies.SelectMany(a => a.ExportedTypes))
            {
                if (typeof(IStartupModule).IsAssignableFrom(type))
                {
                    var instance = Activate(type);
                    StartupModules.Add(instance);
                }
                else if (typeof(IApplicationInitializer).IsAssignableFrom(type))
                {
                    ApplicationInitializers.Add(type);
                }
            }
        }
        public void AddStartupModule<T>(T startupModule) where T : IStartupModule {

            StartupModules.Add(startupModule);
        }

        public void AddStartupModule<T>() where T : IStartupModule {
            AddStartupModule(typeof(T));
        }

        public void AddStartupModule(Type type)
        {
            if (typeof(IStartupModule).IsAssignableFrom(type))
            {
                var instance = Activate(type);
                StartupModules.Add(instance);
            }
            else
            {
                throw new ArgumentException(
                    $"Specified startup module '{type.Name}' does not implement {nameof(IStartupModule)}.",
                    nameof(type));
            }
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="action"></param>
        public void ConfigureMiddleware(Action<IApplicationBuilder, ConfigureMiddlewareContext> action) {
            StartupModules.Add(new InlineMiddlewareConfiguration(action));
        }
      

        private IStartupModule Activate(Type type)
        {
            try
            {
                return (IStartupModule)Activator.CreateInstance(type)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create instance for {nameof(IStartupModule)} type '{type.Name}'.", ex);
            }
        }
    }
}
